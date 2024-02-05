﻿using Senparc.AI.Entities;
using Senparc.AI.Interfaces;
using Senparc.AI.Kernel;
using Senparc.AI.Kernel.Handlers;
using Senparc.CO2NET.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.AI.Samples.Consoles.Samples
{
    public class ChatSample
    {
        IAiHandler _aiHandler;

        SemanticAiHandler _semanticAiHandler => (SemanticAiHandler)_aiHandler;

        public ChatSample(IAiHandler aiHandler)
        {
            _aiHandler = aiHandler;
        }

        public async Task RunAsync()
        {
            await Console.Out.WriteLineAsync(@"ChatSample 开始运行");
            await Console.Out.WriteLineAsync($@"[聊天设置 - 1/2] 请输入机器人系统信息（System Message），默认信息如下，如无需修改可直接输入回车。");
            await Console.Out.WriteLineAsync();
            await Console.Out.WriteLineAsync("------ System Message Start ------");
            await Console.Out.WriteLineAsync(Senparc.AI.DefaultSetting.DEFAULT_SYSTEM_MESSAGE);
            await Console.Out.WriteLineAsync("------  System Message End  ------");
            await Console.Out.WriteLineAsync();

            var systemMessage = Console.ReadLine();
            systemMessage = systemMessage.IsNullOrEmpty() ? Senparc.AI.DefaultSetting.DEFAULT_SYSTEM_MESSAGE : systemMessage;

            int maxHistoryCount = 0;
            do
            {
                await Console.Out.WriteLineAsync("[聊天设置 - 2/2] 请输入最大保留历史对话数量，建议 5-20 之间");
            } while (int.TryParse(Console.ReadLine(), out maxHistoryCount) && maxHistoryCount <= 0);

            await Console.Out.WriteLineAsync();

            await Console.Out.WriteLineAsync(@"配置完成，请输入对话内容。
输入 [ML] 开启单次对话的多行模式
输入 [END] 完成所有多行输入
输入 exit 退出。");

            await Console.Out.WriteLineAsync();

            var parameter = new PromptConfigParameter()
            {
                MaxTokens = 2000,
                Temperature = 0.7,
                TopP = 0.5,
            };

            //await Console.Out.WriteLineAsync(localResponse);
            //var remoteResponse = await huggingFaceRemote.CompleteAsync(Input);
            // modelName: "gpt-4-32k"*/

            var setting = (SenparcAiSetting)Senparc.AI.Config.SenparcAiSetting;//也可以留空，将自动获取

            var chatConfig = _semanticAiHandler.ChatConfig(parameter, 
                                userId: "Jeffrey", 
                                maxHistoryStore: maxHistoryCount,
                                chatSystemMessage:systemMessage, 
                                senparcAiSetting: setting);
            var iWantToRun = chatConfig.iWantToRun;

            var multiLineContent = new StringBuilder();
            var useMultiLine = false;
            //开始对话
            while (true)
            {
                await Console.Out.WriteLineAsync("人类：");
                var prompt = Console.ReadLine();

                if (prompt.ToUpper() == "[ML]")
                {
                    await Console.Out.WriteLineAsync("识别到多行模式，请继续输入");
                    useMultiLine = true;
                }

                while (useMultiLine)
                {
                    if (prompt.ToUpper() == "[END]")
                    {
                        useMultiLine = false;
                        prompt = multiLineContent.ToString();
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync("请继续输入，直到输入 [END] 停止...");
                        prompt = Console.ReadLine();
                        multiLineContent.Append(prompt);
                    }
                }

                if (prompt == "exit")
                {
                    break;
                }

                var dt = SystemTime.Now;

                // Arrange
                if (false)
                {
                    /*
                    var huggingFaceLocal = new HuggingFaceTextCompletion(Model, endpoint: Endpoint);
                    var huggingFaceRemote = new HuggingFaceTextCompletion(Model);

                    AIRequestSettings aiRequestSettings = new AIRequestSettings()
                    {
                        ExtensionData = new Dictionary<string, object>()
                        {
                            { "Temperature", 0.7 },
                            { "TopP", 0.5 },
                            { "MaxTokens", 3000 }
                        }
                    };

                    // Act
                    var localResponse = await huggingFaceLocal.CompleteAsync(prompt, aiRequestSettings);

                    await Console.Out.WriteLineAsync("机器：");
                    await Console.Out.WriteLineAsync(localResponse.ToString());

                    */

                    //await Console.Out.WriteLineAsync("===1=====");
                    //localResponse.ToList().ForEach(x => Console.Write(x));
                }
                else
                {
                    var result = await _semanticAiHandler.ChatAsync(iWantToRun, prompt);

                    await Console.Out.WriteLineAsync("机器：");
                    await Console.Out.WriteLineAsync(result.Output);
                    await Console.Out.WriteLineAsync();
                }
            }
        }
    }
}