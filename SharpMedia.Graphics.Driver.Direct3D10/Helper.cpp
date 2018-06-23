#include "Helper.h"

using namespace SharpMedia::Math;

namespace SharpMedia {
namespace Graphics {
namespace Driver {
namespace Direct3D10 {

  static const char* table[] = 
  {
	"int",
	"int2",
	"int3",
	"int4",
	"uint",
	"uint2",
	"uint3",
	"uint4",
	"float",
	"float2",
	"float3",
	"float4",
	"double",
	"double2",
	"double3",
	"double4",
	"float2x2",
	"float3x3",
	"float4x4",
	"double2x2",
	"double3x3",
	"double4x4",
	"texture1D",
	"texture2D",
	"texture3D",
	"textureCube",
	"texture1DArray",
	"texture2DArray",
	"sampler",
	"buffer",
	"bool",
	"boolx2",
	"boolx3",
	"boolx4"
  };

  const char* ToDXString(PinFormat fmt)
  {
	  

	  switch(fmt)
	  {
	  case PinFormat::Integer:
		  return table[0];
	  case PinFormat::Integerx2:
		  return table[1];
	  case PinFormat::Integerx3:
		  return table[2];
	  case PinFormat::Integerx4:
		  return table[3];
	  case PinFormat::UInteger:
		  return table[4];
	  case PinFormat::UIntegerx2:
		  return table[5];
	  case PinFormat::UIntegerx3:
		  return table[6];
	  case PinFormat::UIntegerx4:
		  return table[7];
	  case PinFormat::Float:
		  return table[8];
	  case PinFormat::Floatx2:
		  return table[9];
	  case PinFormat::Floatx3:
		  return table[10];
	  case PinFormat::Floatx4:
		  return table[11];
	  case PinFormat::Float2x2:
		  return table[16];
	  case PinFormat::Float3x3:
		  return table[17];
	  case PinFormat::Float4x4:
		  return table[18];
	  case PinFormat::Texture1D:
		  return table[22];
	  case PinFormat::Texture2D:
		  return table[23];
	  case PinFormat::Texture3D:
		  return table[24];
	  case PinFormat::TextureCube:
		  return table[25];
	  case PinFormat::Texture1DArray:
		  return table[26];
  	  case PinFormat::Texture2DArray:
		  return table[27];
  	  case PinFormat::Sampler:
		  return table[28];
	  case PinFormat::BufferTexture:
		  return table[29];
  	  case PinFormat::Bool:
		  return table[30];
      case PinFormat::Boolx2:
		  return table[31];
  	  case PinFormat::Boolx3:
		  return table[32];
  	  case PinFormat::Boolx4:
		  return table[33];
	  default:
		  NOT_SUPPORTED();
	  }
   }


   const char* ToDXString(PinComponent component)
   {


	  // Cannot use switch because compiled misbehaves (out of int bounds).
	  if(component == PinComponent::Position)
	  {
		return "SV_Position";
	  } else if(component == PinComponent::PrimitiveID)
	  {
		return "SV_PrimitiveID";
	  } else if(component == PinComponent::InstanceID)
	  {
		return "SV_InstanceID";
	  } else if(component == PinComponent::VertexID)
	  {
		return "SV_VertexID";
	  } else if(component == PinComponent::Depth)
	  {
		return "SV_Depth";
	  } else if(component == PinComponent::RenderTarget0)
	  {
		return "SV_Target0";
	  } else if(component == PinComponent::RenderTarget1)
	  {
		return "SV_Target1";
	  } else if(component == PinComponent::RenderTarget2)
	  {
		return "SV_Target2";
	  } else if(component == PinComponent::RenderTarget3)
	  {
		return "SV_Target3";
	  } else if(component == PinComponent::RenderTarget4)
	  {
		return "SV_Target4";
	  } else if(component == PinComponent::RenderTarget5)
	  {
		return "SV_Target5";
	  } else if(component == PinComponent::RenderTarget6)
	  {
		return "SV_Target6";
	  } else if(component == PinComponent::RenderTarget7)
	  {
		return "SV_Target7";
	  } else if(component == PinComponent::User0)
	  {
		return "NSV_User0_";
	  } else if(component == PinComponent::User1)
	  {
		return "NSV_User1_";
	  } else if(component == PinComponent::User2)
	  {
		return "NSV_User2_";
	  } else if(component == PinComponent::User3)
	  {
		return "NSV_User3_";
	  } else if(component == PinComponent::User4)
	  {
		return "NSV_User4_";
	  } else if(component == PinComponent::User5)
	  {
		return "NSV_User5_";
	  } else if(component == PinComponent::TexCoord0)
	  {
		return "NSV_Texture0_";
	  } else if(component == PinComponent::TexCoord1)
	  {
		return "NSV_Texture1_";
	  } else if(component == PinComponent::TexCoord2)
	  {
		return "NSV_Texture2_";
	  } else if(component == PinComponent::TexCoord3)
	  {
		return "NSV_Texture3_";
	  } else
	  {
		NOT_SUPPORTED();
	  }

   }


   std::string ToDXValue(PinFormat format, Object^ data)
   {
		switch(format)
		{
		case PinFormat::Floatx4:
			{
				Vector4f v = *((Vector4f^)data);
				std::string s = "float4(";
				s += ConvToString(v.X);
				s += ",";
				s += ConvToString(v.Y);
				s += ",";
				s += ConvToString(v.Z);
				s += ",";
				s += ConvToString(v.W);
				s += ")";
				return s;
			}
		case PinFormat::Float:
			{
				Single s = *((Single^)data);
				return ConvToString(s);
			}
		case PinFormat::Floatx2:
			{
				Vector2f v = *((Vector2f^)data);
				std::string s = "float2(";
				s += ConvToString(v.X);
				s += ",";
				s += ConvToString(v.Y);
				s += ")";
				return s;
			}
		case PinFormat::Floatx3:
			{
				Vector3f v = *((Vector3f^)data);
				std::string s = "float3(";
				s += ConvToString(v.X);
				s += ",";
				s += ConvToString(v.Y);
				s += ",";
				s += ConvToString(v.Z);
				s += ")";
				return s;
			}
		case PinFormat::Integer:
			{
				Int32 v = *((Int32^)data);
				return ConvToString(v);
			}
		case PinFormat::UInteger:
			{
				UInt32 v = *((UInt32^)data);
				return ConvToString(v);
			}
		case PinFormat::Integerx2:
			{
				Vector2i v = *((Vector2i^)data);
				std::string s = "int2(";
				s += ConvToString(v.X);
				s += ",";
				s += ConvToString(v.Y);
				s += ")";
				return s;
			}
		case PinFormat::Integerx3:
			{
				Vector3i v = *((Vector3i^)data);
				std::string s = "int3(";
				s += ConvToString(v.X);
				s += ",";
				s += ConvToString(v.Y);
				s += ",";
				s += ConvToString(v.Z);
				s += ")";
				return s;
			}
		case PinFormat::Integerx4:
			{
				Vector4i v = *((Vector4i^)data);
				std::string s = "int4(";
				s += ConvToString(v.X);
				s += ",";
				s += ConvToString(v.Y);
				s += ",";
				s += ConvToString(v.Z);
				s += ",";
				s += ConvToString(v.W);
				s += ")";
				return s;
			}
		}

		NOT_SUPPORTED();
   }


}
}
}
}