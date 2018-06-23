#pragma once
#include <windows.h>
#include <D3D10.h>

using namespace System;
using namespace SharpMedia::Math;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {


	public ref class D3D10RenderTargetView : public IRenderTargetView
	{
	public:	
		ID3D10RenderTargetView* view;
		void Clear(ID3D10Device* device, Colour colour);
		D3D10RenderTargetView(ID3D10RenderTargetView* view);
		virtual ~D3D10RenderTargetView();

	};

	

}
}
}
}