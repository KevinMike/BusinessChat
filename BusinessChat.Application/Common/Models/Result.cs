using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessChat.Application.Common.Models
{
    public class Result<T>
    {
        internal Result(bool succeeded,T content)
        {
            Succeeded = succeeded;
            Content = content;
            Errors = new string[] {};
        }

        internal Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        public bool Succeeded { get; set; }

        public string[] Errors { get; set; }

        public T Content;

        public static Result<T> Success(T content)
        {
            return new Result<T>(true,content);
        }

        public static Result<T> Failure(IEnumerable<string> errors)
        {
            return new Result<T>(false, errors);
        }
    }
}
