using ScreenRecorderLib;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            MyRecorder myRecorder = new MyRecorder();
            myRecorder.CreateRecording();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            myRecorder.EndRecording();
            Console.WriteLine(
                "Recording has been stopped. Press any key to exit...");
            Console.ReadKey();
        }
    }

    class MyRecorder
    {
        Recorder _rec;
        public void CreateRecording()
        {
            string downLoadPath = "C:\\Users\\user\\Downloads";
            string videoPath = Path.Combine(downLoadPath, "test1.mp4");
            var sources = new List<RecordingSourceBase>();
            sources.Add(GetWindow());

            RecorderOptions options = new RecorderOptions
            {
                VideoEncoderOptions = new VideoEncoderOptions
                {
                    Framerate = 60,
                    IsFixedFramerate = true,
                    Bitrate = 1000 * 1000 * 20,
                },
                SourceOptions = new SourceOptions
                {
                    RecordingSources =sources
                }
            };
            _rec = Recorder.CreateRecorder(options);
            _rec.OnRecordingComplete += Rec_OnRecordingComplete;
            _rec.OnRecordingFailed += Rec_OnRecordingFailed;
            _rec.OnStatusChanged += Rec_OnStatusChanged;
            _rec.Record(videoPath);
        }
        public void EndRecording()
        {
            _rec.Stop();
        }
        private void Rec_OnRecordingComplete(object sender, RecordingCompleteEventArgs e)
        {
            //Get the file path if recorded to a file
            string path = e.FilePath;
        }
        private void Rec_OnRecordingFailed(object sender, RecordingFailedEventArgs e)
        {
            string error = e.Error;
        }
        private void Rec_OnStatusChanged(object sender, RecordingStatusEventArgs e)
        {
            RecorderStatus status = e.Status;
        }

        private RecordingSourceBase GetWindow(string title = "")
        {
            var allWindows = Recorder.GetWindows();
            foreach (var window in allWindows)
            {
                if(window.Title == title)
                {
                    return window;
                }
            }

            // if title is not found, return main monitor
            return new DisplayRecordingSource(DisplayRecordingSource.MainMonitor);
        }
    }
}
