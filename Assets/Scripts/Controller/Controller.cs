using System;
using System.Collections.Generic;
using UnityEngine;

namespace RModeling.Controller
{
    public class Controller : MonoBehaviour
    {
        private Queue<ICommand> commands;

        private ICommand currentCommand;

        private ControllerStatus status;

        private void Start()
        {
            commands = new Queue<ICommand>();
        }

        private void Update()
        {
            if (this.status == ControllerStatus.Executing)
            {
                Step(Time.deltaTime);
            }
        }

        private void Run()
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
            currentCommand = commands.Dequeue();
        }

        public void AddCommand(ICommand command)
        {
            if (commands == null)
            {
                throw new ArgumentNullException("Commands queue is null.");
            }

            commands.Enqueue(command);
        }
    }

    public interface ICommand
    {
        CommandStatus Execute(float deltaTime);
    }

    public enum CommandStatus
    {
        Done,
        Failed,
        Executing
    }

    public enum ControllerStatus
    {
        Stop,
        Executing,
    }
}