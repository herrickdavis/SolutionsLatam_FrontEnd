using System;
using System.Collections.Generic;

namespace AlsGlobal.Models
{
  public class ErrorViewModel
  {
    public int ErrorCode { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
  }
  public class ResponseResult
  {
    public ResponseResult()
    {
      Errors = new ResponseErrorMessages();
    }

    public string Title { get; set; }
    public int Status { get; set; }
    public ResponseErrorMessages Errors { get; set; }
    public void AgregarError(string error)
    {
      Errors.Message.Add(error);
    }
  }

  public class ResponseErrorMessages
  {
    public ResponseErrorMessages()
    {
      Message = new List<string>();
    }

    public List<string> Message { get; set; }
  }
}
