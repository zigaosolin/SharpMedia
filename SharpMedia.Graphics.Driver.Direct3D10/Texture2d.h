#pragma once
#include <windows.h>
#include <D3D10.h>

using namespace System;
using namespace SharpMedia::Math;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {


	public ref class D3D10Texture2d : public ITexture2D
	{
	public:
		ID3D10Texture2D* texture2D;
	public:
        virtual array<Byte>^ Read(UInt32 mipmap, UInt32 face);
        virtual void Update(array<Byte>^ data, UInt32 mipmap, UInt32 face);
		D3D10Texture2d(ID3D10Texture2D* texture);
		virtual ~D3D10Texture2d();
	};

	public ref class D3D10TextureView : public ITextureView
	{
	public:
		ID3D10ShaderResourceView* view;
	public:
		D3D10TextureView(ID3D10ShaderResourceView* view);
		virtual ~D3D10TextureView();
	};

	
}
}
}
}