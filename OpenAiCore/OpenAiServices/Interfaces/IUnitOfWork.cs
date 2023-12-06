using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAiCore.OpenAiServices.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }
}
