﻿using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel;
using Senparc.AI.Kernel.Entities;
using Senparc.AI.Kernel.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.AI.Kernel.Handlers
{

    public class IWantTo
    {
        public IKernel Kernel { get; set; }
        public KernelConfig KernelConfig { get; set; }
        public SemanticKernelHelper SemanticKernelHelper { get; set; }

        public string UserId { get; set; }
        public string ModelName { get; set; }

        public IWantTo() { }

        public IWantTo(KernelConfig kernelConfig)
        {
            KernelConfig = kernelConfig;
        }

        public IWantTo(SemanticKernelHelper semanticKernelHelper)
        {
            SemanticKernelHelper = semanticKernelHelper;
        }


        //public IWantTo Config(string userId, string modelName)
        //{
        //    UserId = userId;
        //    ModelName = modelName;
        //    return this;
        //}
    }

    public class IWantToConfig
    {
        public IWantTo IWantTo { get; set; }
        public string UserId { get; set; }
        public string ModelName { get; set; }

        public IWantToConfig(IWantTo iWantTo)
        {
            IWantTo = iWantTo;
        }
    }

    public class IWanToRun
    {
        public IWantTo IWantTo { get; set; }
        public ISKFunction ISKFunction { get; set; }
        public SenparcAiContext AiContext { get; set; }
        public IWanToRun(IWantTo iWantTo)
        {
            IWantTo = iWantTo;
        }
    }
}
