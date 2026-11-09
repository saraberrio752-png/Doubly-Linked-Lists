using BACKEND;

namespace BACKEND
{
    public class piece<t>
    {
        public t value;
        public piece<t> next;
        public piece<t> prev;
        public piece(t Value) => value = Value;
    }
}

public class doublyLinkedList<t> where t : IComparable<t>
{
    public piece<t> head;
    public piece<t> tail;
    public bool ascending = true;
    public int count { get; private set; } // Property to keep track of the number of elements in the list

    private int Compare(t a, t b)
    {
        return ascending ? a.CompareTo(b) : b.CompareTo(a);
    }

 public void add(t value)
    {
        piece<t> newPiece = new piece<t>(value);
        if (head == null)
        {
            head = tail = newPiece;
        }
        else
        {
            piece<t> current = head;
            while (current != null && Compare(current.value, value) < 0) //little 
            {
                current = current.next;
            }
            if (current == head)
            {
                newPiece.next = head;
                head.prev = newPiece;
                head = newPiece;
            }
            else if (current == null)
            {
                tail.next = newPiece;
                newPiece.prev = tail;
                tail = newPiece;
            }
            else
            {
                newPiece.prev = current.prev;
                newPiece.next = current;
                current.prev.next = newPiece;
                current.prev = newPiece;
            }
        }
        count = count + 1; // Increment the count when a new element is added
    }
    
    public int Count()
    {
        return count; // Return the number of elements in the list
    }

    public IEnumerable<t> forward()  // Go through the list forward +1
    {
        piece<t> current = head;
        while (current != null)
        {
            yield return current.value;  // Return the current value and pause with that value
            current = current.next;
        }
    }

 public IEnumerable<t> backward()  // Go through the list backward -1
    {
        piece<t> current = tail;
        while (current != null)
        {
            yield return current.value;  // Return the current value and pause with that value
            current = current.prev;
        }
    }

    public void sortdescending()
    {
        ascending = false;
        // Rebuild the list in descending order
        var value2 = new List<t>(forward());
        value2.Sort((a, b) => b.CompareTo(a)); // Sort in descending order
        head = tail = null; // the ends empty out and the insides get erased like a domino effect
        count = 0; // Reset the count
        foreach (var value in value2)
        {
            add(value); // Add each value back to the list
        }
    }

    public List<(t value, int count)> countduplicates() //(value , count)
    {
        var duplicates = new List<(t value, int count)>();
        if (head == null) return duplicates;
        piece<t> current = head;
        while (current != null)
        {
            int duplicateCount = 1;
            piece<t> nextPiece = current.next;
            while (nextPiece != null && current.value.CompareTo(nextPiece.value) == 0)
            {
                duplicateCount++;
                nextPiece = nextPiece.next;
            }
            duplicates.Add((current.value, duplicateCount));
            current = nextPiece;
        }
        return duplicates;
    }


    public bool exists(t value)
    {
        piece<t> current = head;
        while (current != null)
        {
            int result = Compare (current.value, value);

        if (result == 0)   return true; // Value exists in the list
            
         if ( result > 0) return false; // Value does not exist in the list (since the list is sorted) -1  it's too much

            current = current.next;
        }
        return false; // Value does not exist in the list
    }
    private void joiningofends(piece<t> current)
    {
        if (current.prev != null)
        {
            current.prev.next = current.next;
        }
        else
        {
            head = current.next; // Update head if the first element is removed
        }
        if (current.next != null)
        {
            current.next.prev = current.prev;
        }
        else
        {
            tail = current.prev; // Update tail if the last element is removed
        }
        count = count -  1; // Decrement the count when an element is removed

    }

public bool removefirst(t value)
    {
        piece<t> current = head;
        while (current != null)
        {
            if (current.value.CompareTo(value) == 0)
            {
                joiningofends(current);
                return true; // Value found and removed
            }
            current = current.next;
        }
        return false; // Value not found in the list
    }

    public int removeall(t value)
    {
        int removed = 0;
        piece<t> current = head;
        while (current != null)
        {
            piece<t> nextPiece = current.next;
            if (current.value.CompareTo(value) == 0)
            {
                joiningofends(current);
                removed++;
            }
            current = nextPiece;
        }
        return removed;
    }
}
