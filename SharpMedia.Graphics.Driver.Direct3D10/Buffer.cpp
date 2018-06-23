#include "Buffer.h"
#include "Helper.h"

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	IVBufferView^ D3D10Buffer::CreateVView(ID3D10Device* device, unsigned int stride, UInt64 offset)
	{
		return gcnew D3D10VBuffer(this, stride, offset);	
	}

	IIBufferView^ D3D10Buffer::CreateIView(ID3D10Device* device, bool wide, UInt64 offset)
	{
		return gcnew D3D10IBuffer(this, wide, offset);
	}

	ICBufferView^ D3D10Buffer::CreateCView(ID3D10Device* device)
	{
		return gcnew D3D10CBuffer(this);
	}

	array<Byte>^ D3D10Buffer::Read(UInt64 offset, UInt64 count)
	{
		array<Byte>^ data = gcnew array<Byte>((int)count);
		Byte* ptr;

		// We map the buffer.
		DXFAILED(buffer->Map(D3D10_MAP_READ, 0, (void**)&ptr));


		Common::Memcpy(ptr, data, count);

		buffer->Unmap();

		return data;
	}

	void D3D10Buffer::Update(array<Byte>^ data, UInt64 offset, UInt64 count)
	{
		Byte* ptr;

		// We update the buffer.
		DXFAILED(buffer->Map(D3D10_MAP_WRITE_DISCARD, 0, (void**)&ptr))

		Common::Memcpy(data, ptr, count);

		buffer->Unmap();
	}

	D3D10Buffer::D3D10Buffer(ID3D10Buffer* buffer)
	{
		this->buffer = buffer;
	}

	D3D10Buffer::~D3D10Buffer()
	{
		buffer->Release();
	}

// ---------------------------------------------------------------------------------------
// Views
// ---------------------------------------------------------------------------------------

	void D3D10VBuffer::Apply(ID3D10Device* device, unsigned int index)
	{
		// Must copy to enable pass by pointer.
		unsigned int off = (unsigned int)offset;
		unsigned int str = stride;
		ID3D10Buffer* buf = buffer->buffer;
		device->IASetVertexBuffers(index, 1, &buf, &str, &off);
	}

	D3D10VBuffer::D3D10VBuffer(D3D10Buffer^ buffer, unsigned int stride, UInt64 offset)
	{
		this->buffer = buffer;
		this->stride = stride;
		this->offset = offset;
	}

	D3D10VBuffer::~D3D10VBuffer()
	{
	}


	void D3D10IBuffer::Apply(ID3D10Device* device)
	{
		unsigned int off = (unsigned int)offset;
		device->IASetIndexBuffer(buffer->buffer, wide ? DXGI_FORMAT_R32_UINT : DXGI_FORMAT_R16_UINT, off);
	}

	D3D10IBuffer::D3D10IBuffer(D3D10Buffer^ buffer, bool wide, UInt64 offset)
	{
		this->buffer = buffer;
		this->wide = wide;
		this->offset = offset;
	}

	D3D10IBuffer::~D3D10IBuffer()
	{
	}



	ID3D10Buffer* D3D10CBuffer::GetBuffer()
	{
		return buffer->buffer;
	}

	D3D10CBuffer::D3D10CBuffer(D3D10Buffer^ buffer)
	{
		this->buffer = buffer;
	}

	D3D10CBuffer::~D3D10CBuffer()
	{
	}

}
}
}
}