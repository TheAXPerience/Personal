#pragma once
#ifndef LINKEDLISTDEBUG
#define LINKEDLISTDEBUG 10

#include <iostream>
#include <stdexcept>
#include "MyLinkNode.h"
using namespace std;

template <typename T>
class MyLinkedList
{
	template <typename T> friend ostream& operator<<(ostream &, const MyLinkedList<T> &);
public:
	MyLinkedList();
	MyLinkedList<T>& add(T value);
	MyLinkedList<T>& remove(unsigned int index);
	T get(unsigned int index) const;
	unsigned int size() const;
	~MyLinkedList();
private:
	MyLinkNode<T> *front;
	MyLinkNode<T> *end;

	unsigned int list_size;
};

template <typename T>
ostream& operator<<(ostream& output, const MyLinkedList<T>& list)
{
	MyLinkNode<T> *curr = list.front;
	while (curr != NULL)
	{
		output << curr->value << endl;
		curr = curr->next;
	}
	return output;
}

template <class T>
MyLinkedList<T>::MyLinkedList()
{
	front = NULL;
	end = NULL;
	list_size = 0;
}

template <class T>
MyLinkedList<T>& MyLinkedList<T>::add(T value)
{
	MyLinkNode<T> *temp = new MyLinkNode<T>(value, NULL);
	if (front == NULL)
	{
		front = temp;
		end = temp;
	}
	else
	{
		end->next = temp;
		end = temp;
	}
	list_size++;
	return *this;
}

template <class T>
MyLinkedList<T>& MyLinkedList<T>::remove(unsigned int index)
{
	if (index >= list_size)
	{
		//exception; out of bounds
		return *this;
	}
	if (index == 0 && front != NULL)
	{
		MyLinkNode<T> *temp = front;
		front = temp->next;
		delete temp;
	}
	else if (index > 0 && front != NULL)
	{
		unsigned int i;
		MyLinkNode<T> *prev = front;
		for (i = 1; i < index; i++)
		{
			prev = prev->next;
		}
		MyLinkNode<T> *temp = prev->next;
		prev->next = temp->next;
		delete temp;
	}
	list_size--;
	return *this;
}

template <class T>
T MyLinkedList<T>::get(unsigned int index) const
{
	if (index == 0)
	{
		return front->value;
	}
	else if (index < list_size)
	{
		MyLinkNode<T> *curr = front->next;
		for (unsigned int i = 1; i < index; i++)
		{
			curr = curr->next;
		}
		return curr->value;
	}
	else
	{
		throw runtime_error("Out of bounds exception");
	}
}

template <class T>
unsigned int MyLinkedList<T>::size() const
{
	return list_size;
}

template <class T>
MyLinkedList<T>::~MyLinkedList()
{
	while (front != NULL)
	{
		end = front->next;
		delete front;
		front = end;
	}
}

#endif