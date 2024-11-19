
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ReadMemory;


public class MemoryReader
{
    const int PROCESS_WM_READ = 0x0010;
    //const int PROCESS_ALL_ACCESS = 0x1fffff;

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

    private IntPtr ProcessHandle { get; init; }

    public MemoryReader(string processName)
    {
        Process process = Process.GetProcessesByName(processName)[0];
        ProcessHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);
    }

    public void ReadMemory()
    {
        // First VirtualQueryEx ?
        // see https://github.com/FssAy/dc_mem_grabber/blob/master/src/main.rs

        int baseAddress = 0;
        byte[] buffer = new byte[1024 * 1024];
        while (true)
        {
            int bytesRead = 0;
            ReadProcessMemory((int)ProcessHandle, baseAddress, buffer, buffer.Length, ref bytesRead);
            Console.WriteLine(Encoding.Unicode.GetString(buffer) + " (" + bytesRead.ToString() + "bytes)");
            baseAddress += buffer.Length;
        }
    }

    // implement IDisposable ?

}
