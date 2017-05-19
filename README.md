# Transmissions - Single Entity Socket Communication in C#

Transmissions allows you to send and receive strictly matching data.

## Usage

### Establishing your Data Model

This library sends byte data via an interface class called `ITransmissible`.

By implementing it you define how your transmitter and receiver will interpret byte data.
`int GetByteLength()` - returning the total byte length of your data.
`void Serialize(Stream)` - passing byte data to the `Stream`
`void Deserialize(byte[])` - reading the byte array and filling your own object.

````cs
public class Message : ITransmissible
{

	public int value;

	public void Deserialize(byte[] array)
	{
		this.value = BitConverter.ToInt32(array, 0);
	}

	public int GetByteLength()
	{
		return 4;
	}

	public void Serialize(Stream stream)
	{
		stream.Write(BitConverter.GetBytes(value), 0, 4);
	}
}
````

In the example above we implement a class that holds an integer.

### Transmitting Data

We transmit data by instantiating a `Transmitter` with the desired Ip Address and port, and invoking `Send(ITransmissible)`.

````cs
Transmitter transmitter = new Transmitter("127.0.0.1", 1234);

Message message = new Message
{
	value = 12
};

transmitter.Send(message);
````

### Receiving Data

First we need a `ITransmissionListener` to handle our incoming messages.

````cs
public class MessageListener : ITransmissionListener
{
	public ITransmissible InstantiateTransmissible()
	{
		return new Message();
	}

	public void OnReceive(ITransmissible received)
	{
		Console.WriteLine((received as Message).value);
	}
}
````

The method `InstantiateTransmissible()` instantiates a brand new object for the receiver to fill up with data.
`OnReceive(ITransmissible)` will be called whenever a new object is received.

We receive data by instantiating a `Receiver` and invoking `StartService(ITransmissionListener)`.

````cs
Receiver receiver = new Receiver(2255);
receiver.StartService(new MessageListener());
````

## Install Library

__Step 1.__ Get this code and compile it

##  License

MIT License. See the file LICENSE.md with the full license text.

## Author

[![Caio Comandulli](https://avatars3.githubusercontent.com/u/3738961?v=3&s=150)](https://github.com/caiocomandulli "On Github")

Copyright (c) 2016 Caio Comandulli
