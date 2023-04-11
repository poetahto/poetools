using System;
using System.Collections.Generic;
using System.Linq;
using poetools.Console.Commands;
using poetools.Core;
using poetools.Core.Abstraction;
using UnityEngine;

namespace poetools.Console
{
    // NOT DOABLE: move init method into base class
    // DONE: allow customization of command prefix format
    // todo: draggable and close-box view
    // todo: command options and auto-complete for them
    // todo: clean up
    // todo: do singleton / static pass on codebase to ensure quick-play-mode compatibility

    // INVARIANTS - autoCompleteIndex should always be in range of autoCompleteCommands
    // INVARIANTS - all reference-type fields must not be null.

    public class RuntimeConsole : PreparedSingleton<RuntimeConsole>
    {
        public CommandRegistry CommandRegistry { get; private set; }
        public RuntimeConsoleView View { get; private set; }

        private IInputHistory InputHistory { get; set; }
        private static RuntimeConsoleSettings _settings;
        private int _autoCompleteIndex;

        internal static event Action OnCreate;
        public event Action RegistrationFinished;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            // Try to initialize user settings.
            _settings = Resources.Load<RuntimeConsoleSettings>("RuntimeConsoleSettings");

            if (_settings == null)
            {
                // Initialize default settings
                _settings = Resources.Load<RuntimeConsoleSettings>(
                    RuntimeConsoleNaming.ResourcesMenuName + "/RuntimeConsoleSettings"
                );
            }

            if (_settings.AutoCreate)
                Prepare();
        }

        protected override void Awake()
        {
            base.Awake();

            CommandRegistry = new CommandRegistry();
            InputHistory = new InputHistory(_settings.MaxCommandHistory);

            // Initialize the view.
            View = Instantiate(_settings.ConsoleViewPrefab, transform);
            View.name = _settings.ConsoleViewPrefab.name;
            View.Hide();

            // Register default commands.
            foreach (Command command in _settings.AutoRegisterCommands)
            {
                command.Initialize(this);
                CommandRegistry.Register(command);
            }

            OnCreate?.Invoke();
            RegistrationFinished?.Invoke();
        }

        private void OnDestroy()
        {
            CommandRegistry.Dispose();
            OnCreate = null;
        }

        #region Event Handling

        private void OnEnable()
        {
            View.OnVisibilityChanged += HandleVisibilityChange;
            View.InputFieldDisplay.OnSubmit += HandleSubmit;
            View.InputFieldDisplay.OnValueChange += HandleInputChange;
        }

        private void OnDisable()
        {
            View.OnVisibilityChanged -= HandleVisibilityChange;
            View.InputFieldDisplay.OnSubmit -= HandleSubmit;
            View.InputFieldDisplay.OnValueChange -= HandleInputChange;
        }

        private void HandleSubmit(string input)
        {
            string[] splitInput = input.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);

            if (input.Length > 0 && splitInput.Length > 0)
            {
                string[] args = ArgumentTools.Parse(splitInput);
                ICommand command = CommandRegistry.FindCommand(splitInput[0]);
                View.Text += '\n' + _settings.UserPrefix.GenerateMessage(input);

                command.Execute(args, this);
                InputHistory.AddEntry(input);
            }

            ResetInputField();
        }

        private CursorLockMode _previousLockMode;
        private bool _previousIsVisible;

        private void HandleVisibilityChange(bool wasVisible, bool isVisible)
        {
            if (isVisible)
            {
                _previousLockMode = Cursor.lockState;
                _previousIsVisible = Cursor.visible;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = _previousLockMode;
                Cursor.visible = _previousIsVisible;
            }

            if (isVisible)
                ResetInputField();
        }

        private List<string> _suggestions = new List<string>();

        private void HandleInputChange(string oldValue, string newValue)
        {
            string[] splitInput = newValue.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);

            if (splitInput.Length == 0)
            {
                // case: we have no input.
                View.AutoCompleteDisplay.SetText("");
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                var newSpaceCount = newValue.Count(c => c == ' ');
                var oldSpaceCount = oldValue.Count(c => c == ' ');

                // If we entered a space, apply the auto-complete.
                if (_suggestions.Any() && newSpaceCount > oldSpaceCount)
                {
                    _autoCompleteIndex = 0;
                    string autoCompleteText = View.AutoCompleteDisplay.GetText();
                    View.InputFieldDisplay.SetText(autoCompleteText + " ");
                }
            }

            CommandRegistry.FindCommands(View.InputFieldDisplay.GetText(), _suggestions);
            UpdateAutoCompleteText();
        }

        #endregion

        private void UpdateAutoCompleteText()
        {
            var index = (int) Mathf.Repeat(_autoCompleteIndex, _suggestions.Count);
            string autoCompleteText = _suggestions.Count > 0 ? _suggestions[index] : "";
            View.AutoCompleteDisplay.SetText(autoCompleteText);
        }

        private void CycleAutoCompleteOption(int direction)
        {
            _autoCompleteIndex += direction;
            UpdateAutoCompleteText();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && InputHistory.TryMoveBackwards(out string prevCommand))
                View.InputFieldDisplay.SetText(prevCommand);

            if (Input.GetKeyDown(KeyCode.DownArrow) && InputHistory.TryMoveForwards(out string nextCommand))
                View.InputFieldDisplay.SetText(nextCommand);

            if (Input.GetKeyDown(KeyCode.Tab))
                CycleAutoCompleteOption(1);
        }

        public void Log(string category, string message)
        {
            string header = _settings.LogPrefix.GenerateMessage(category);
            View.Text += $"\n{header}{message}";
        }

        private void ResetInputField()
        {
            View.InputFieldDisplay.Clear();
            View.InputFieldDisplay.Focus();
            InputHistory.Clear();
        }
    }
}
