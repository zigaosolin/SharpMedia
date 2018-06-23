#pragma once
#include <windows.h>
#include <D3D10.h>

using namespace System;
using namespace SharpMedia::Math;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {


	public ref class D3D10Buffer : public IBuffer
	{
	public:
		ID3D10Buffer* buffer;
	
		
		// View creations:
		IVBufferView^ CreateVView(ID3D10Device* device, unsigned int stride, UInt64 offset);
		IIBufferView^ CreateIView(ID3D10Device* device, bool wide, UInt64 offset);
		ICBufferView^ CreateCView(ID3D10Device* device);

		// More views later: texture 1D, texture buffer, render target, depth stencil
		

		virtual array<Byte>^ Read(UInt64 offset, UInt64 count);
        virtual void Update(array<Byte>^ data, UInt64 offset, UInt64 count);

		D3D10Buffer(ID3D10Buffer* buffer);
		virtual ~D3D10Buffer();

	};

	public ref class D3D10VBuffer : public IVBufferView
	{
		D3D10Buffer^ buffer;
		unsigned int stride;
		UInt64 offset;
	public:
		void Apply(ID3D10Device* device, unsigned int index);
		D3D10VBuffer(D3D10Buffer^ buffer, unsigned int stride, UInt64 offset);
		virtual ~D3D10VBuffer();
	};

	public ref class D3D10IBuffer : public IIBufferView
	{
		D3D10Buffer^ buffer;
		bool wide;
		UInt64 offset;
	public:
		void Apply(ID3D10Device* device);
		D3D10IBuffer(D3D10Buffer^ buffer, bool wide, UInt64 offset);
		virtual ~D3D10IBuffer();
	};

	public ref class D3D10CBuffer : ICBufferView
	{
		D3D10Buffer^ buffer;
	public:
		ID3D10Buffer* GetBuffer();
		D3D10CBuffer(D3D10Buffer^ buffer);
		virtual ~D3D10CBuffer();
	};
	

	

}
}
}
}