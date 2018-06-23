#include "Texture2d.h"
#include "Helper.h"

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	D3D10TextureView::D3D10TextureView(ID3D10ShaderResourceView* view)
	{
		this->view = view;
	}

	D3D10TextureView::~D3D10TextureView()
	{
		view->Release();
	}


	array<Byte>^ D3D10Texture2d::Read(UInt32 mipmap, UInt32 face)
	{
		D3D10_TEXTURE2D_DESC desc;
		texture2D->GetDesc( &desc );

		// We compute width/height.
		UInt32 width = desc.Width >> mipmap;
		UInt32 height = desc.Height >> mipmap;

		width = width > 0 ? width : 1;
		height = height > 0 ? height : 1;


		array<Byte>^ res = gcnew array<Byte>(width * height * ToFormatSize(desc.Format));

		D3D10_MAPPED_TEXTURE2D mapped;
		texture2D->Map(D3D10CalcSubresource(mipmap, face, desc.MipLevels), D3D10_MAP_READ, 0, &mapped);

		// We compute the number of actual bytes in one element.
		UInt32 bytesPerElement = ToFormatSize(desc.Format);
	
		// We fill data.
		Byte* src = (Byte*)mapped.pData;
		UInt32 i = 0;
		for( UInt32 row = 0; row < height; row++ )
		{
			UInt32 rowStart = row * mapped.RowPitch;
			for( UInt32 col = 0; col < width * bytesPerElement; col++ )
			{
				res[i++] = src[col];
			}
		}

		texture2D->Unmap(D3D10CalcSubresource(mipmap, face, desc.MipLevels));

		return res;
	}


    void D3D10Texture2d::Update(array<Byte>^ data, UInt32 mipmap, UInt32 face)
	{
		D3D10_TEXTURE2D_DESC desc;
		texture2D->GetDesc( &desc );

		D3D10_MAPPED_TEXTURE2D mapped;
		texture2D->Map(D3D10CalcSubresource(mipmap, face, desc.MipLevels), D3D10_MAP_WRITE, 0, &mapped);

		// Compute the width and height.
		UInt32 width = desc.Width >> mipmap;
		UInt32 height = desc.Height >> mipmap;

		if(width == 0) width = 1;
		if(height == 0) height = 1;

		// We compute the number of actual bytes in one element.
		UInt32 bytesPerElement = data->Length / (width*height);
	
		// We fill data.
		Byte* dst = (Byte*)mapped.pData;
		UInt32 i = 0;
		for( UInt32 row = 0; row < height; row++ )
		{
			UInt32 rowStart = row * mapped.RowPitch;
			for( UInt32 col = 0; col < width * bytesPerElement; col++ )
			{
				dst[col] = data[i++];
			}
		}

		texture2D->Unmap(D3D10CalcSubresource(mipmap, face, desc.MipLevels));

	}

	D3D10Texture2d::D3D10Texture2d(ID3D10Texture2D* texture)
	{
		texture2D = texture;
	}

	D3D10Texture2d::~D3D10Texture2d()
	{
		texture2D->Release();
	}

}
}
}
}