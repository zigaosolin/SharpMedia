#pragma once
#include <windows.h>
#include <D3D10.h>

using namespace System;
using namespace SharpMedia::Math;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {


	public ref class D3D10DepthStencilTargetView : public IDepthStencilTargetView
	{
	public:		
		ID3D10DepthStencilView* view;
		void Clear(ID3D10Device* device, ClearOptions op, float depth, unsigned int stencil);

		D3D10DepthStencilTargetView(ID3D10DepthStencilView* view);
		virtual ~D3D10DepthStencilTargetView();

	};

	

}
}
}
}