using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NAPS2.Wia;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace RestApiWithServiceWorker.Domain;

public interface IWiaDevices
{
    Task<List<string>> GetWiaDevices();
    Task<bool> Scan(Scanner scanner);
}

public class WiaDevice : IWiaDevices
{
    private readonly ILogger<WiaDevice> _logger;

    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")] 
    private Dictionary<Bitmap, string> data = new();
    private Scanner curScanner { get; set; }

    private string CurFile { get; set; }

    public WiaDevice(ILogger<WiaDevice> logger)
    {
        _logger = logger;
    }

    private Dictionary<string, string> WiaDevices { get; set; }


    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
    private void ScanDevices()
    {
        if (WiaDevices != null) return;

        try
        {
            // WiaDeviceManager доступен только на Windows
            using var deviceManager = new WiaDeviceManager();
            WiaDevices = deviceManager.GetDeviceInfos()
                .ToDictionary(k => (string)k.Properties.FirstOrDefault(e => e.Name.Equals("Name"))?.Value,
                    v => (string)v.Properties.FirstOrDefault(e => e.Name.Equals("Unique Device ID"))?.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError("Ошибка в поиске устройств WIA - {0}", ex);
        }
    }

    public async Task<List<string>> GetWiaDevices()
    {
        await Task.Factory.StartNew(ScanDevices);
        return WiaDevices.Keys.ToList();
    }

    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
    public NAPS2.Wia.WiaDevice GetWiaDeviceByName(string name)
    {
        if (!WiaDevices.ContainsKey(name))
        {
            _logger.LogError("Сканер не найден в словаре");
            return null;
        }

        var value = WiaDevices[name] as string;

        using var deviceManager = new WiaDeviceManager();
        return string.IsNullOrEmpty(value) ? null : deviceManager.FindDevice(value);
    }

    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
    public async Task<bool> Scan(Scanner scanner)
    {
        _logger.LogInformation("Подготовка к сканированию...");
        data.Clear();
        CurFile = string.Empty;

        if (scanner.isDebug)
        {
            scanner.File = "Debug.pdf";
            return false;
        }
        
        var curWIA = GetWiaDeviceByName(scanner.Name);

        if (curWIA == null)
        {
            _logger.LogError("Сканер не найден в списке WIA");
            return false;
        }

        scanner.Id = curWIA.Id();
        curScanner = scanner;

        await ClearDir();

        var subItem = scanner.IsFeeder ? "Feeder" : "Auto";
        var item = curWIA.FindSubItem(subItem);

        item?.SetProperty(WiaPropertyId.IPS_PAGES, WiaPropertyValue.ALL_PAGES);

        if (scanner.isDuplex)
        {
            try
            {
                item?.SetProperty(WiaPropertyId.IPS_DOCUMENT_HANDLING_SELECT,
                    WiaPropertyValue.DUPLEX);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка в настройке дуплекса - {0}", ex);
            }
        }

        using var transfer = item?.StartTransfer();

        if (transfer == null)
        {
            _logger.LogError("Ошибка в начале трансфера данных.");
            return false;
        }

        transfer.PageScanned += PageScanned;
        transfer.TransferComplete += TransferComplete;

        try
        {
            await Download(transfer);
        }
        catch (Exception ex)
        {
            _logger.LogError("Ошибка в сканировании - {0}", ex);
            return false;
        }

        scanner.File = CurFile;
        _logger.LogInformation("Сканирование завершено...");
        await ClearDir();
        return true;
    }


    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
    private async Task Download(WiaTransfer transfer)
    {
        await Task.Factory.StartNew(transfer.Download);
    }


    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
    private void PageScanned(object sender, WiaTransfer.PageScannedEventArgs e)
    {
        using (e.Stream)
        {
            try
            {
                var bitmap = new Bitmap(e.Stream);

                var format = curScanner.Format.Equals("jpeg") ? ImageFormat.Jpeg: ImageFormat.Png;
                var ext = curScanner.Format.Equals("jpeg") ? "jpeg" : "png";
                var imageExtension = $".{ext}";

                var random = new Random();
                var path = Path.Combine(Path.GetTempPath(),
                    "naumen" + (int)DateTime.Now.TimeOfDay.TotalMilliseconds + "_tempoScanner" + random.Next(2, 1000) + imageExtension);

                bitmap.Save(path, format);
                data?.Add(bitmap, path);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка в обработке данных сканирования - {0}", ex);
            }
        }
    }

    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
    private void TransferComplete(object sender, EventArgs e)
    {
        var format = curScanner.Format;

        switch (data.Count)
        {
            case 0:
                return;
            case 1:
            {
                if (format.Equals("pdf"))
                {
                    SavePdf(format);
                    return;
                }
                foreach (var kv in data)
                {
                    var path = kv.Value;
                    var replace = Regex.Replace(path, "(_tempoScanner)\\d+", "");
                    var fName = replace.Split(Path.DirectorySeparatorChar).LastOrDefault();
                    if (File.Exists(replace))
                        File.Delete(replace);
                    File.Move(path, replace);
                    CurFile = fName;
                }
                return;
            }
            default:
                SavePdf(format);
                break;
        }
    }

    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
    private void SavePdf(string format)
    {
        using var pdfDocument = new PdfDocument();

        foreach (var kv in data)
        {
            var page = pdfDocument.AddPage();

            var gfx = XGraphics.FromPdfPage(page);
            var img = XImage.FromFile(kv.Value);
            gfx.DrawImage(img, 0, 0);
            page.Close();
        }

        var fName = $"naumen{DateTime.Now.Millisecond.ToString()}.pdf";

        var path = Path.Combine(Path.GetTempPath(), fName);
        if (File.Exists(path))
            File.Delete(path);
        pdfDocument.Save(path);
        CurFile = fName;
    }


    public async Task ClearDir()
    {
        var files = Directory.GetFiles(Path.GetTempPath());
        await Task.Factory.StartNew(() =>
        {
            foreach (var file in files)
            {
                if (!file.Contains("_tempoScanner")) continue;
                if (File.Exists(file))
                    File.Delete(file);
            }
        });
    }
}