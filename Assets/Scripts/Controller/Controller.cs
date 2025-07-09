using System;
using System.Collections.Generic;
using UnityEngine;

namespace RModeling.Controller
{
    public class Controller : MonoBehaviour
    {
        private Queue<ICommand> commands = new Queue<ICommand>();

        private ICommand currentCommand;

        private ControllerStatus status;

        private void Update()
        {
            if (this.status == ControllerStatus.Executing)
            {
                Step(Time.deltaTime);
            }
        }

        public void Run()
        {
            if (commands == null)
            {
                throw new ArgumentNullException("Commands queue is null.");
            }

            if (this.status == ControllerStatus.Stop)
            {
                TakeNextCommand();
                this.status = ControllerStatus.Executing;
            }
        }

        private void Step(float deltaTime)
        {
            var status = currentCommand.Execute(deltaTime);

            switch (status)
            {
                case CommandStatus.Executing:
                    this.status = ControllerStatus.Executing;
                    break;
                case CommandStatus.Done:
                    TakeNextCommand();
                    break;
                case CommandStatus.Failed:
                    this.status = ControllerStatus.Stop;
                    break;
            }
        }

        private void TakeNextCommand()
        {
            if (commands.Count > 0)
            {
                currentCommand = commands.Dequeue();
                currentCommand.Init();
            } else
            {
                this.status = ControllerStatus.Stop;
            }
        }

        public void AddCommand(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("Commands queue is null.");
            }

            commands.Enqueue(command);
        }
    }
}