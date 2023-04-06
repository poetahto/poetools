using System.Collections.Generic;

namespace poetools.Console
{
    public interface IInputHistory
    {
        void AddEntry(string input);
        bool TryMoveBackwards(out string previous);
        bool TryMoveForwards(out string next);
    }
    
    public class InputHistory : IInputHistory
    {
        // invariant 1: commandHistory never has more nodes that maxHistoryLength
        
        private readonly LinkedList<string> _commandHistory;
        private readonly int _maxHistoryLength;

        private LinkedListNode<string> _currentHistoryNode;

        public InputHistory(int maxHistoryLength)
        {
            _maxHistoryLength = maxHistoryLength;
            _commandHistory = new LinkedList<string>();
            _currentHistoryNode = new LinkedListNode<string>("");
        }

        public void AddEntry(string input)
        {
            _commandHistory.AddLast(input);
            _currentHistoryNode = _commandHistory.Last;
                    
            // Maintain invariant 1
            while (_commandHistory.Count > _maxHistoryLength)
                _commandHistory.RemoveFirst();
        }

        public bool TryMoveBackwards(out string result)
        {
            return TryMoveTo(_currentHistoryNode.Previous, out result);
        }

        public bool TryMoveForwards(out string result)
        {
            return TryMoveTo(_currentHistoryNode.Next, out result);
        }

        private bool TryMoveTo(LinkedListNode<string> destination, out string result)
        {
            result = _currentHistoryNode.Value;

            if (destination == null)
                return false;

            result = destination.Value;
            _currentHistoryNode = destination;
            return true;
        }
    }
}