﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace RestApiWithServiceWorker.Controller
{
    public class HealthCheck : IHealthCheck
    {
        public HealthCheck()
        {
        }

        public Task<HealthCheckResult> CheckHealthAsync
            (HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var healthCheckResultHealthy = true;

            if (healthCheckResultHealthy)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("A healthy result."));
            }

            return Task.FromResult(
                new HealthCheckResult(context.Registration.FailureStatus,
                "An unhealthy result."));
        }
    }
}
