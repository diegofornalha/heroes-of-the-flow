using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class AwaitExtensions
{
  public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
  {
    var tcs = new TaskCompletionSource<object>();
    asyncOp.completed += obj => { tcs.SetResult(null); };
    return ((Task)tcs.Task).GetAwaiter();
  }
  public static TaskAwaiter GetAwaiter(this UnityWebRequestAsyncOperation webReqOp)
  {
    var tcs = new TaskCompletionSource<object>();
    webReqOp.completed += obj =>
    {
      {
        if (webReqOp.webRequest.responseCode != 200)
        {
          tcs.SetException(new FileLoadException(webReqOp.webRequest.error));
        }
        else
        {
          tcs.SetResult(null);
        }
      }
    };
    return ((Task)tcs.Task).GetAwaiter();
  }

  // public static TaskAwaiter<int> GetAwaiter(this Process process)
  // {
  //   var tcs = new TaskCompletionSource<int>();
  //   process.EnableRaisingEvents = true;

  //   process.Exited += (s, e) => tcs.TrySetResult(process.ExitCode);

  //   if (process.HasExited)
  //   {
  //     tcs.TrySetResult(process.ExitCode);
  //   }

  //   return tcs.Task.GetAwaiter();
  // }

  // public static async void WrapErrors(this Task task)
  // {
  //   await task;
  // }
}
