#pragma once
#ifndef NODE_OF_THE_LEGENDARY_BOMB
#define NODE_OF_THE_LEGENDARY_BOMB

template <class T>
class MyLinkNode
{
public:
	const T value;
	MyLinkNode<T> *next;

	MyLinkNode(T, MyLinkNode<T> *);
	~MyLinkNode();
};

template <class T>
MyLinkNode<T>::MyLinkNode(T v, MyLinkNode<T> *n)
	: value(v), next(n)
{
	
}

template <class T>
MyLinkNode<T>::~MyLinkNode()
{
	
}

#endif