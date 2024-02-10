
#region
//Console.Write("Performing some task... ");
//using (var progress = new ProgressBar())
//{
//    for (int i = 0; i <= 100; i++)
//    {
//        progress.Report((double)i / 100);
//        Thread.Sleep(20);
//    }
//}
//Console.WriteLine("Done.");
#endregion


using System.Management;

ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
ManagementObjectCollection collection = searcher.Get();
string username = (string)collection.Cast<ManagementBaseObject>().First()["UserName"];
Console.WriteLine(username);