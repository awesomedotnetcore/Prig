﻿/* 
 * File: ChatRoomTest.cs
 * 
 * Author: Akira Sugiura (urasandesu@gmail.com)
 * 
 * 
 * Copyright (c) 2015 Akira Sugiura
 *  
 *  This software is MIT License.
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */


using NUnit.Framework;
using program1.MyLibrary;
using System.Collections.Generic;
using UntestableLibrary;
using UntestableLibrary.Prig;
using Urasandesu.Prig.Framework;

namespace Test.program1.MyLibrary
{
    [TestFixture]
    public class ChatRoomTest
    {
        delegate bool ReplyInformationFunc(ULChatBot @this, ULTypeOfFeedback input, out string reply, ref ULActions action, out ULTypeOfFeedback recommendation);

        [Test]
        public void Start_on_execute_should_return_expected()
        {
            using (new IndirectionsContext())
            {
                // Arrange
                var actualInputs = new List<ULTypeOfFeedback>();

                var proxy1 = new Proxy<OfPProxyULChatBot>();
                proxy1.Setup<ReplyInformationFunc>(_ => _.ReplyInformationULTypeOfFeedbackStringRefULActionsRefULTypeOfFeedbackRef()).Body =
                    (ULChatBot @this, ULTypeOfFeedback input, out string reply, ref ULActions action, out ULTypeOfFeedback recommendation) =>
                    {
                        actualInputs.Add(input);
                        reply = "1";
                        recommendation = ULTypeOfFeedback.Suggestion;
                        return true;
                    };

                var proxy2 = new Proxy<OfPProxyULChatBot>();
                proxy2.Setup<ReplyInformationFunc>(_ => _.ReplyInformationULTypeOfFeedbackStringRefULActionsRefULTypeOfFeedbackRef()).Body =
                    (ULChatBot @this, ULTypeOfFeedback input, out string reply, ref ULActions action, out ULTypeOfFeedback recommendation) =>
                    {
                        actualInputs.Add(input);
                        reply = "2";
                        recommendation = ULTypeOfFeedback.Incomprehensible;
                        return true;
                    };


                // Act
                new ChatRoom().Start(ULTypeOfFeedback.Praise, (ULChatBot)proxy1.Target, (ULChatBot)proxy2.Target);


                // Assert
                var expectedInputs = new[] { ULTypeOfFeedback.Praise, ULTypeOfFeedback.Suggestion };
                CollectionAssert.AreEqual(expectedInputs, actualInputs);
            }
        }
    }
}
