﻿using System;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;

namespace Coding4Fun.Toolkit.Audio
{
	public class MicrophoneRecorder : Recorder<InMemoryRandomAccessStream>
	{
		private MediaCapture _mediaCap;

		private async Task<bool> InitMicrophone()
		{
			if (Buffer != null)
				Buffer.Dispose();

			Buffer = new InMemoryRandomAccessStream();

			if (_mediaCap != null)
				return true;
			
			try
			{
				var settings = new MediaCaptureInitializationSettings {StreamingCaptureMode = StreamingCaptureMode.Audio};
				_mediaCap = new MediaCapture();

				await _mediaCap.InitializeAsync(settings);

				_mediaCap.RecordLimitationExceeded += RecordLimitationExceeded;
				_mediaCap.Failed += Failed;
			}
			catch (Exception ex0)
			{
				if (ex0.InnerException != null && ex0.InnerException.GetType() == typeof (UnauthorizedAccessException))
					throw ex0.InnerException;
				
				throw;
			}

			return true;
		}

		public async override void Start()
		{
			await InitMicrophone();
			await _mediaCap.StartRecordToStreamAsync(MediaEncodingProfile.CreateM4a(AudioEncodingQuality.Auto), Buffer);

			base.Start();
		}

		public async override void Stop()
		{
			await _mediaCap.StopRecordAsync();

			base.Stop();
		}

		private void Failed(MediaCapture currentCaptureObject, MediaCaptureFailedEventArgs currentFailure)
		{
			CatastrophicFailure = true;
			FailureException = new Exception(string.Format("Code: {0}. {1}", currentFailure.Code, currentFailure.Message));

			base.Stop();
		}

		private void RecordLimitationExceeded(MediaCapture currentCaptureObject)
		{
			FailureException = new Exception("Exceeded Record Limitation");

			Stop();
		}
	}
}