using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using NAPS2.Wia;

namespace RestApiWithServiceWorker.Domain;

public interface IScanner
{
    Task<List<string>> GetWiaDevices();
}

public class Scanner : IScanner
{
    private Dictionary<string, string> WiaDevice { get; set; }


    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
    private void ScanDevices()
    {
        if (WiaDevice != null) return;

        // WiaDeviceManager доступен только на Windows
        using var deviceManager = new WiaDeviceManager();

        WiaDevice = deviceManager.GetDeviceInfos()
            .ToDictionary(k => (string)k.Properties.FirstOrDefault(e => e.Name.Equals("Name"))?.Value,
                v => (string)v.Properties.FirstOrDefault(e => e.Name.Equals("Unique Device ID"))?.Value);
    }

    public async Task<List<string>> GetWiaDevices()
    {
        return await Task.Factory.StartNew(() => WiaDevice.Keys.ToList());
    }

    [SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
    private WiaDevice GetWiaDeviceByName(string name)
    {
        if (!WiaDevice.ContainsKey(name)) return null;
        var value = WiaDevice[name] as string;

        using var deviceManager = new WiaDeviceManager();
        return string.IsNullOrEmpty(value) ? null : deviceManager.FindDevice(value);
    }
}