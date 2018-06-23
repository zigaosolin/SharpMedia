#pragma once
#include <windows.h>
#include <D3D10.h>
#include <string>
#include <cstdlib>


using namespace System;
using namespace SharpMedia;
using namespace SharpMedia::Graphics;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

	
#define NOT_SUPPORTED() throw gcnew NotSupportedException("Option not (yet?) supported.");
#define DXFAILED(x) if(FAILED(x)) { throw gcnew Exception("Method failed."); }

   inline static DXGI_FORMAT ToDXFormat(CommonPixelFormatLayout format)
   {
	  switch(format)
	  {
	  case CommonPixelFormatLayout::X8Y8Z8W8_UNORM:
		  return DXGI_FORMAT_R8G8B8A8_UNORM;
	  case CommonPixelFormatLayout::X8Y8Z8W8_TYPELESS:
		  return DXGI_FORMAT_R8G8B8A8_TYPELESS;
	  default:
		  NOT_SUPPORTED();
	  }
   }

   inline static UINT8 ToDXWriteMask(States::WriteMask mask)
   {
	  UINT8 m = 0;
	  if((int)mask & (int)States::WriteMask::Red) m |= D3D10_COLOR_WRITE_ENABLE_RED;
	  if((int)mask & (int)States::WriteMask::Green) m |= D3D10_COLOR_WRITE_ENABLE_GREEN;
	  if((int)mask & (int)States::WriteMask::Blue) m |= D3D10_COLOR_WRITE_ENABLE_BLUE;
	  if((int)mask & (int)States::WriteMask::Alpha) m |= D3D10_COLOR_WRITE_ENABLE_ALPHA;
	  return m;
   }

   inline static D3D10_BLEND_OP ToDXOperation(States::BlendOperation op)
   {
	  switch(op)
	  {
	  case States::BlendOperation::Add:
		  return D3D10_BLEND_OP_ADD;
      case States::BlendOperation::Subtract:
		  return D3D10_BLEND_OP_SUBTRACT;
	  case States::BlendOperation::ReverseSubtract:
		  return D3D10_BLEND_OP_REV_SUBTRACT;
	  case States::BlendOperation::Min:
		  return D3D10_BLEND_OP_MIN;
	  case States::BlendOperation::Max:
		  return D3D10_BLEND_OP_MAX;
	  }

	  NOT_SUPPORTED();

   }

   inline static D3D10_BLEND ToDXOperand(States::BlendOperand op)
   {
	   switch(op)
	   {
	   case States::BlendOperand::Zero:
		   return D3D10_BLEND_ZERO;
	   case States::BlendOperand::One:
		   return D3D10_BLEND_ONE;
	   case States::BlendOperand::SrcColour:
		   return D3D10_BLEND_SRC_COLOR;
	   case States::BlendOperand::SrcColourInverse:
		   return D3D10_BLEND_INV_SRC_COLOR;
	   case States::BlendOperand::SrcAlpha:
		   return D3D10_BLEND_SRC_ALPHA;
	   case States::BlendOperand::SrcAlphaInverse:
		   return D3D10_BLEND_INV_SRC_ALPHA;
	   case States::BlendOperand::DstAlpha:
		   return D3D10_BLEND_DEST_ALPHA;
	   case States::BlendOperand::DstAlphaInverse:
		   return D3D10_BLEND_INV_DEST_ALPHA;
	   case States::BlendOperand::DstColour:
		   return D3D10_BLEND_DEST_COLOR;
	   case States::BlendOperand::DstColourInverse:
		   return D3D10_BLEND_INV_DEST_COLOR;
	   case States::BlendOperand::BlendFactor:
		   return D3D10_BLEND_BLEND_FACTOR;
	   case States::BlendOperand::BlendFactorInverse:
		   return D3D10_BLEND_INV_BLEND_FACTOR;
	   }

	   NOT_SUPPORTED();
   }

   inline static D3D10_CULL_MODE ToDXMode(States::CullMode mode)
   {
		switch(mode)
		{
		case States::CullMode::Back:
			return D3D10_CULL_BACK;
		case States::CullMode::Front:
			return D3D10_CULL_FRONT;
		case States::CullMode::None:
			return D3D10_CULL_NONE;
		}

		NOT_SUPPORTED();
   }

   inline static D3D10_FILL_MODE ToDXMode(States::FillMode mode)
   {
		switch(mode)
		{
		case States::FillMode::Solid:
			return D3D10_FILL_SOLID;
		case States::FillMode::Wireframe:
			return D3D10_FILL_WIREFRAME;
		}

		NOT_SUPPORTED();
   }

   inline static D3D10_STENCIL_OP ToDXOperation(States::StencilOperation op)
   {
		switch(op)
		{
		case States::StencilOperation::Keep:
			return D3D10_STENCIL_OP_KEEP;
		case States::StencilOperation::Zero:
			return D3D10_STENCIL_OP_ZERO;
		case States::StencilOperation::Replace:
			return D3D10_STENCIL_OP_REPLACE;
		case States::StencilOperation::Increase:
			return D3D10_STENCIL_OP_INCR;
		case States::StencilOperation::Decrease:
			return D3D10_STENCIL_OP_DECR;
		case States::StencilOperation::IncreaseSAT:
			return D3D10_STENCIL_OP_INCR_SAT;
		case States::StencilOperation::DecreaseSAT:
			return D3D10_STENCIL_OP_DECR_SAT;
		case States::StencilOperation::Invert:
			return D3D10_STENCIL_OP_INVERT;
		}

		NOT_SUPPORTED();
   }

   inline static D3D10_COMPARISON_FUNC ToDXFunc(States::CompareFunction compare)
   {

	   switch(compare)
	   {
	   case States::CompareFunction::Never:
		   return D3D10_COMPARISON_NEVER;
	   case States::CompareFunction::Always:
		   return D3D10_COMPARISON_ALWAYS;
	   case States::CompareFunction::Equal:
		   return D3D10_COMPARISON_EQUAL;
	   case States::CompareFunction::Less:
		   return D3D10_COMPARISON_LESS;
	   case States::CompareFunction::LessEqual:
		   return D3D10_COMPARISON_LESS_EQUAL;
	   case States::CompareFunction::Greater:
		   return D3D10_COMPARISON_GREATER;
	   case States::CompareFunction::GreaterEqual:
		   return D3D10_COMPARISON_GREATER_EQUAL;
	   case States::CompareFunction::NotEqual:
		   return D3D10_COMPARISON_NOT_EQUAL;
	   }

	   NOT_SUPPORTED();
   }


   inline static D3D10_TEXTURE_ADDRESS_MODE ToDXAddress(States::AddressMode mode)
   {
		switch(mode)
		{
		case States::AddressMode::Border:
			return D3D10_TEXTURE_ADDRESS_BORDER;
		case States::AddressMode::Wrap:
			return D3D10_TEXTURE_ADDRESS_WRAP;
		case States::AddressMode::Mirror:
			return D3D10_TEXTURE_ADDRESS_MIRROR;
		case States::AddressMode::MirrorOnce:
			return D3D10_TEXTURE_ADDRESS_MIRROR_ONCE;
		case States::AddressMode::Clamp:
			return D3D10_TEXTURE_ADDRESS_CLAMP;
		}
	   NOT_SUPPORTED();
   }

   inline static D3D10_FILTER ToDXFilter(States::Filter filter, States::Filter mipmapFilter)
   {
		switch(filter)
		{
		case States::Filter::Point:
			switch(filter)
			{
			case States::Filter::Point:
				return D3D10_FILTER_MIN_MAG_MIP_POINT;
			case States::Filter::Linear:
				return D3D10_FILTER_MIN_MAG_POINT_MIP_LINEAR;
			case States::Filter::Anisotropic:
				return D3D10_FILTER_ANISOTROPIC;
			}
		case States::Filter::Linear:
			switch(filter)
			{
			case States::Filter::Point:
				return D3D10_FILTER_MIN_MAG_LINEAR_MIP_POINT;
			case States::Filter::Linear:
				return D3D10_FILTER_MIN_MAG_MIP_LINEAR;
			case States::Filter::Anisotropic:
				return D3D10_FILTER_ANISOTROPIC;
			}
		case States::Filter::Anisotropic:
			return D3D10_FILTER_ANISOTROPIC;
		}
	    NOT_SUPPORTED();
   }

   const char* ToDXString(PinFormat fmt);
   const char* ToDXString(PinComponent component);

   inline static std::string ToDXSwizzle(SwizzleMask^ mask)
   {
	   std::string r;
	   
	   // For now unsupported matrix swizzling.
	   if(mask->RowCount != 1)
	   {
			throw gcnew NotImplementedException();
	   }

	   for(UInt32 i = 0; i < mask->ColumnCount; i++)
	   {
		   switch(mask[i])
		   {
		   case SwizzleMask::ComponentSelector::X:
			   r.append("x");
			   break;
		   case SwizzleMask::ComponentSelector::Y:
			   r.append("y");
			   break;
		   case SwizzleMask::ComponentSelector::Z:
			   r.append("z");
			   break;
		   case SwizzleMask::ComponentSelector::W:
			   r.append("w");
			   break;
		   default:
			   throw gcnew NotImplementedException();
		   }
	   }
	   

	   return r;
   }

   inline static const char* ToDXComponent(PinComponent component, unsigned int& semanticIndex)
   {
		semanticIndex = 0;
		return ToDXString(component);
   }

   inline static UInt32 ToFormatSize(DXGI_FORMAT fmt)
   {
	  switch(fmt)
	  {
	  case DXGI_FORMAT_R32_FLOAT:
	  case DXGI_FORMAT_R32_UINT:
	  case DXGI_FORMAT_R32_SINT:
	  case DXGI_FORMAT_R32_TYPELESS:
		return 4;
	  case DXGI_FORMAT_R32G32_FLOAT:
	  case DXGI_FORMAT_R32G32_UINT:
	  case DXGI_FORMAT_R32G32_SINT:
	  case DXGI_FORMAT_R32G32_TYPELESS:
		return 4 * 2;
	  case DXGI_FORMAT_R32G32B32_FLOAT:
	  case DXGI_FORMAT_R32G32B32_UINT:
	  case DXGI_FORMAT_R32G32B32_SINT:
	  case DXGI_FORMAT_R32G32B32_TYPELESS:
		return 4 * 3;
	  case DXGI_FORMAT_R32G32B32A32_FLOAT:
	  case DXGI_FORMAT_R32G32B32A32_UINT:
	  case DXGI_FORMAT_R32G32B32A32_SINT:
	  case DXGI_FORMAT_R32G32B32A32_TYPELESS:
		return 4 * 4;
	  case DXGI_FORMAT_R8_UINT:
	  case DXGI_FORMAT_R8_SINT:
	  case DXGI_FORMAT_R8_TYPELESS:
	  case DXGI_FORMAT_R8_UNORM:
	  case DXGI_FORMAT_R8_SNORM:
		  return 1;
	  case DXGI_FORMAT_R8G8_UINT:
	  case DXGI_FORMAT_R8G8_SINT:
	  case DXGI_FORMAT_R8G8_TYPELESS:
	  case DXGI_FORMAT_R8G8_UNORM:
	  case DXGI_FORMAT_R8G8_SNORM:
		  return 2;
  	  case DXGI_FORMAT_R8G8B8A8_UINT:
	  case DXGI_FORMAT_R8G8B8A8_SINT:
	  case DXGI_FORMAT_R8G8B8A8_TYPELESS:
	  case DXGI_FORMAT_R8G8B8A8_UNORM:
	  case DXGI_FORMAT_R8G8B8A8_SNORM:
		  return 4;


	  default:
		  NOT_SUPPORTED();
	  }
   }

   inline static DXGI_FORMAT ToDXFormat(PinFormat fmt)
   {
		switch(fmt)
		{
		case PinFormat::Float:
			return DXGI_FORMAT_R32_FLOAT;
		case PinFormat::Floatx2:
			return DXGI_FORMAT_R32G32_FLOAT;
		case PinFormat::Floatx3:
			return DXGI_FORMAT_R32G32B32_FLOAT;
		case PinFormat::Floatx4:
			return DXGI_FORMAT_R32G32B32A32_FLOAT;
		case PinFormat::Integer:
			return DXGI_FORMAT_R32_SINT;
		case PinFormat::Integerx2:
			return DXGI_FORMAT_R32G32_SINT;
		case PinFormat::Integerx3:
			return DXGI_FORMAT_R32G32B32_SINT;
		case PinFormat::Integerx4:
			return DXGI_FORMAT_R32G32B32A32_SINT;
		case PinFormat::UInteger:
			return DXGI_FORMAT_R32_UINT;
		case PinFormat::UIntegerx2:
			return DXGI_FORMAT_R32G32_UINT;
		case PinFormat::UIntegerx3:
			return DXGI_FORMAT_R32G32B32_UINT;
		case PinFormat::UIntegerx4:
			return DXGI_FORMAT_R32G32B32A32_UINT;
		default:
			NOT_SUPPORTED();
		}
   }

   inline static unsigned int ToDXBindFlags(BufferUsage usage)
   {
		unsigned int flags = 0;
		if((int)usage & (int)BufferUsage::ConstantBuffer) flags |= D3D10_BIND_CONSTANT_BUFFER;
		if((int)usage & (int)BufferUsage::GeometryOutput) flags |= D3D10_BIND_STREAM_OUTPUT;
		if((int)usage & (int)BufferUsage::IndexBuffer) flags |= D3D10_BIND_INDEX_BUFFER;
		if((int)usage & (int)BufferUsage::RenderTarget) flags |= D3D10_BIND_RENDER_TARGET;
		if((int)usage & (int)BufferUsage::VertexBuffer) flags |= D3D10_BIND_VERTEX_BUFFER;
		return flags;
   }

   inline static D3D10_USAGE ToDXUsage(Usage usage)
   {
	   switch(usage)
	   {
	   case Usage::Default:
		   return D3D10_USAGE_DEFAULT;
	   case Usage::Staging:
		   return D3D10_USAGE_STAGING;
	   case Usage::Static:
		   return D3D10_USAGE_IMMUTABLE;
	   case Usage::Dynamic:
		   return D3D10_USAGE_DYNAMIC;
	   default:
		   NOT_SUPPORTED();
	   }
   }

   inline static unsigned int ToDXCPUAccess(CPUAccess access)
   {
	   switch(access)
	   {
	   case CPUAccess::None:
		   return 0;
	   case CPUAccess::Read:
		   return D3D10_CPU_ACCESS_READ;
	   case CPUAccess::Write:
		   return D3D10_CPU_ACCESS_WRITE;
	   case CPUAccess::ReadWrite:
		   return D3D10_CPU_ACCESS_READ|D3D10_CPU_ACCESS_WRITE;
	   default:
		   NOT_SUPPORTED();
	   }
   }

   std::string ToDXValue(PinFormat format, Object^ data);

   static inline std::string ConvToString(int t)
   {	
	  char tmp[10];
	  sprintf(tmp, "%d", t);
	  return tmp;
   }

   static inline std::string ConvToString(unsigned int t)
   {	
	  char tmp[10];
	  sprintf(tmp, "%d", (int)t);
	  return tmp;
   }

   static inline std::string ConvToString(float t)
   {
	  char tmp[10];
	  sprintf(tmp, "%f", t);
	  return tmp;
   }

   static inline std::string ConvToString(PinFormat fmt)
   {
	  switch(fmt)
	  {
	  case PinFormat::Float:
		  return "float";
	  case PinFormat::Floatx2:
		  return "float2";
	  case PinFormat::Floatx3:
		  return "float3";
	  case PinFormat::Floatx4:
		  return "float4";
	  case PinFormat::Integer:
		  return "int";
	  case PinFormat::Integerx2:
		  return "int2";
	  case PinFormat::Integerx3:
		  return "int3";
	  case PinFormat::Integerx4:
		  return "int4";
	  case PinFormat::UInteger:
		  return "uint";
	  case PinFormat::UIntegerx2:
		  return "uint2";
	  case PinFormat::UIntegerx3:
		  return "uint3";
	  case PinFormat::UIntegerx4:
		  return "uint4";
	  default:
		  NOT_SUPPORTED();

	  }
   }
	

}
}
}
}