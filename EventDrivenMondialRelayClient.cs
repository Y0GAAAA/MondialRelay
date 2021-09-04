using MondialRelay.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MondialRelay
{

    public class EventDrivenMondialRelayClient : IDisposable
    {

        private readonly MondialRelayClient client;
        private readonly Thread watchdogThread;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Event that is synchronously raised when the delivery state changes.
        /// </summary>
        public event EventHandler<PackageDeliveryProgress> DeliveryStateChanged;
        
        /// <summary>
        /// Amortized delay between delivery state update verification.
        /// Measured in milliseconds, defaults to one minute.
        /// </summary>
        public uint UpdateDelay { get; set; } = 60_000;

        public EventDrivenMondialRelayClient(ulong trackingNumber, int postalCode)
        {
            this.client = new MondialRelayClient(trackingNumber, postalCode);
            this.DeliveryStateChanged += (_, _) => throw new NotImplementedException("DeliveryStateChanged event has not been defined");
            this.watchdogThread = new Thread(() => deliveryStateChangedWatchdogLoop(cancellationTokenSource.Token));
            this.watchdogThread.Start();
        }

        private async void deliveryStateChangedWatchdogLoop(CancellationToken cancellationToken)
        {
            PackageDeliveryProgress lastProgress = await client.GetLastCompletedTask();
            while (!cancellationToken.IsCancellationRequested)
            {

                // (the current delivery progress, the time taken to query the progress)
                var (currentProgress, queryMillisecondDuration) = await client.GetLastCompletedTask().GetExecutionTime();

                // if state has changed: raise event
                if (currentProgress != lastProgress)
                {
                    lastProgress = currentProgress;
                    DeliveryStateChanged.Invoke(this, currentProgress);
                }

                int sleep = (int)(UpdateDelay - queryMillisecondDuration);

                // sleep if needed
                if (sleep > 0)
                    await Task.Delay(sleep, cancellationToken);
            }
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            watchdogThread.Join();
        }
    }

}
