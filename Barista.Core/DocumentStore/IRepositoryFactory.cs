﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Barista.DocumentStore
{
  public interface IRepositoryFactory
  {
    object CreateRepository();

    object CreateRepository(IDocumentStore documentStore);
  }
}
