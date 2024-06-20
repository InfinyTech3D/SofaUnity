// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:1,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:7,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:255,stmr:255,stmw:255,stcp:6,stps:0,stfa:4,stfz:4,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3435,x:33076,y:32872,varname:node_3435,prsc:2|diff-9090-OUT,diffpow-9090-OUT,spec-4215-OUT,gloss-4753-OUT,normal-2630-OUT,emission-1948-OUT,amspl-4892-OUT,alpha-9182-OUT,refract-3834-OUT,disp-3074-OUT,tess-9134-OUT;n:type:ShaderForge.SFN_Color,id:4117,x:30674,y:31862,ptovrint:False,ptlb:[Base] Color,ptin:_BaseColor,varname:node_4117,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.3,c2:0.5,c3:0.9,c4:0.5;n:type:ShaderForge.SFN_Tex2d,id:1720,x:30033,y:33458,ptovrint:False,ptlb:[Primary] Heightmap,ptin:_PrimaryHeightmap,varname:node_1720,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:228b2c301ad35c9449ab45f3112f090e,ntxv:2,isnm:False|UVIN-1258-OUT;n:type:ShaderForge.SFN_TexCoord,id:8565,x:29636,y:33458,varname:node_8565,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:3520,x:30020,y:34344,ptovrint:False,ptlb:[Secondary] Heightmap,ptin:_SecondaryHeightmap,varname:_RefractionTexture_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:228b2c301ad35c9449ab45f3112f090e,ntxv:2,isnm:False|UVIN-4177-OUT;n:type:ShaderForge.SFN_ToggleProperty,id:2710,x:30431,y:34206,ptovrint:False,ptlb:[Secondary] Enable This?,ptin:_SecondaryEnableThis,varname:node_2710,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True;n:type:ShaderForge.SFN_Add,id:1258,x:29833,y:33458,varname:node_1258,prsc:2|A-8565-UVOUT,B-9640-OUT;n:type:ShaderForge.SFN_Multiply,id:9640,x:29636,y:33616,varname:node_9640,prsc:2|A-2123-OUT,B-1131-T;n:type:ShaderForge.SFN_Append,id:2123,x:29349,y:33618,varname:node_2123,prsc:2|A-4156-X,B-4156-Y;n:type:ShaderForge.SFN_Vector4Property,id:4156,x:29133,y:33618,ptovrint:False,ptlb:[Primary] U and V speed (Z W Not used),ptin:_PrimaryUandVspeedZWNotused,varname:node_4156,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1,v2:0.1,v3:0,v4:0;n:type:ShaderForge.SFN_TexCoord,id:911,x:29639,y:34344,varname:node_911,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:1131,x:29315,y:34033,varname:node_1131,prsc:2;n:type:ShaderForge.SFN_Add,id:4177,x:29822,y:34344,varname:node_4177,prsc:2|A-911-UVOUT,B-3813-OUT;n:type:ShaderForge.SFN_Multiply,id:3813,x:29639,y:34501,varname:node_3813,prsc:2|A-6185-OUT,B-1131-T;n:type:ShaderForge.SFN_Append,id:6185,x:29392,y:34490,varname:node_6185,prsc:2|A-2398-X,B-2398-Y;n:type:ShaderForge.SFN_Vector4Property,id:2398,x:29169,y:34490,ptovrint:False,ptlb:[Secondary] U and V speed (Z W Not used),ptin:_SecondaryUandVspeedZWNotused,varname:_UandVspeedZandWnoaffects_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-0.05,v2:-0.075,v3:0,v4:0;n:type:ShaderForge.SFN_Tex2d,id:9209,x:30674,y:32032,ptovrint:False,ptlb:[Base] Texture,ptin:_BaseTexture,varname:node_9209,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4988,x:31219,y:31891,varname:node_4988,prsc:2|A-4117-RGB,B-9209-RGB;n:type:ShaderForge.SFN_Multiply,id:1274,x:31219,y:32049,varname:node_1274,prsc:2|A-4117-A,B-9209-A;n:type:ShaderForge.SFN_ToggleProperty,id:4049,x:31240,y:32348,ptovrint:False,ptlb:[Base] is as Emission,ptin:_BaseisasEmission,varname:node_4049,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False;n:type:ShaderForge.SFN_Slider,id:9021,x:32295,y:32239,ptovrint:False,ptlb:[Overall] Metallic,ptin:_OverallMetallic,varname:node_9021,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.9128559,max:1;n:type:ShaderForge.SFN_Slider,id:4753,x:32942,y:32739,ptovrint:False,ptlb:[Overall] Gloss,ptin:_OverallGloss,varname:node_4753,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.796635,max:1;n:type:ShaderForge.SFN_Slider,id:7952,x:31087,y:33413,ptovrint:False,ptlb:[Overall] Normal Strength,ptin:_OverallNormalStrength,varname:node_7952,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.7343698,max:1;n:type:ShaderForge.SFN_Add,id:744,x:31249,y:34359,varname:node_744,prsc:2|A-1189-OUT,B-1229-OUT;n:type:ShaderForge.SFN_NormalVector,id:8455,x:31369,y:34411,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:3074,x:31839,y:34387,varname:node_3074,prsc:2|A-744-OUT,B-8455-OUT,C-3631-OUT;n:type:ShaderForge.SFN_Slider,id:3631,x:31496,y:34481,ptovrint:False,ptlb:[Overall] Wave Height,ptin:_OverallWaveHeight,varname:node_3631,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2.826293,max:30;n:type:ShaderForge.SFN_Tex2d,id:4769,x:30033,y:33254,ptovrint:False,ptlb:[Primary] Normalmap,ptin:_PrimaryNormalmap,varname:node_4769,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:6ed99089d8d6dbf4bbeb2ecef32d71c1,ntxv:3,isnm:True|UVIN-1258-OUT;n:type:ShaderForge.SFN_Tex2d,id:5281,x:30020,y:34114,ptovrint:False,ptlb:[Secondary] Normalmap,ptin:_SecondaryNormalmap,varname:_NormalMap_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:6ed99089d8d6dbf4bbeb2ecef32d71c1,ntxv:3,isnm:True|UVIN-4177-OUT;n:type:ShaderForge.SFN_Add,id:88,x:31083,y:33239,varname:node_88,prsc:2|A-4769-RGB,B-1908-OUT;n:type:ShaderForge.SFN_Multiply,id:1229,x:30651,y:34317,varname:node_1229,prsc:2|A-4067-OUT,B-2710-OUT;n:type:ShaderForge.SFN_Multiply,id:101,x:31401,y:33239,varname:node_101,prsc:2|A-3594-OUT,B-7952-OUT;n:type:ShaderForge.SFN_Multiply,id:1908,x:30651,y:33949,varname:node_1908,prsc:2|A-5281-RGB,B-2710-OUT;n:type:ShaderForge.SFN_ComponentMask,id:3594,x:31244,y:33239,varname:node_3594,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-88-OUT;n:type:ShaderForge.SFN_Append,id:2630,x:31563,y:33239,varname:node_2630,prsc:2|A-101-OUT,B-6082-OUT;n:type:ShaderForge.SFN_Vector1,id:6082,x:31401,y:33393,varname:node_6082,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:6400,x:31474,y:32240,varname:node_6400,prsc:2|A-4988-OUT,B-4049-OUT;n:type:ShaderForge.SFN_Cubemap,id:5108,x:30076,y:32305,ptovrint:False,ptlb:[Cubemap] Custom Cubemap,ptin:_CubemapCustomCubemap,varname:node_5108,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,pvfc:0|MIP-9766-OUT;n:type:ShaderForge.SFN_Multiply,id:4892,x:30398,y:32546,varname:node_4892,prsc:2|A-6819-OUT,B-5108-A,C-7702-OUT,D-871-OUT,E-3491-OUT;n:type:ShaderForge.SFN_Vector1,id:3491,x:29895,y:32674,varname:node_3491,prsc:2,v1:8;n:type:ShaderForge.SFN_Slider,id:7702,x:29641,y:32567,ptovrint:False,ptlb:[Cubemap] Strength,ptin:_CubemapStrength,varname:node_7702,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.4807122,max:1;n:type:ShaderForge.SFN_Fresnel,id:871,x:29829,y:32407,varname:node_871,prsc:2|EXP-3028-OUT;n:type:ShaderForge.SFN_Slider,id:3028,x:29481,y:32450,ptovrint:False,ptlb:[Cubemap] Fresnel,ptin:_CubemapFresnel,varname:node_3028,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:4.316328,max:20;n:type:ShaderForge.SFN_FragmentPosition,id:3166,x:32649,y:34687,varname:node_3166,prsc:2;n:type:ShaderForge.SFN_ViewPosition,id:7547,x:32649,y:34835,varname:node_7547,prsc:2;n:type:ShaderForge.SFN_Distance,id:9149,x:32835,y:34835,varname:node_9149,prsc:2|A-3166-XYZ,B-7547-XYZ;n:type:ShaderForge.SFN_ValueProperty,id:2599,x:32835,y:34757,ptovrint:False,ptlb:[Tessellation] Near Cap,ptin:_TessellationNearCap,varname:node_2599,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:2989,x:32835,y:34655,ptovrint:False,ptlb:[Tessellation] Far Cap,ptin:_TessellationFarCap,varname:node_2989,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:20;n:type:ShaderForge.SFN_Slider,id:8585,x:32757,y:35017,ptovrint:False,ptlb:[Tessellation] Strength,ptin:_TessellationStrength,varname:node_8585,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:2.48368,max:100;n:type:ShaderForge.SFN_InverseLerp,id:6126,x:33021,y:34655,varname:node_6126,prsc:2|A-2989-OUT,B-2599-OUT,V-9149-OUT;n:type:ShaderForge.SFN_Multiply,id:2281,x:33075,y:34835,varname:node_2281,prsc:2|A-6126-OUT,B-8585-OUT;n:type:ShaderForge.SFN_Max,id:9134,x:33288,y:34590,varname:node_9134,prsc:2|A-1650-OUT,B-2281-OUT;n:type:ShaderForge.SFN_Vector1,id:1650,x:32649,y:34588,varname:node_1650,prsc:2,v1:1;n:type:ShaderForge.SFN_Slider,id:2105,x:30753,y:32950,ptovrint:False,ptlb:[Blend] Blend Distance,ptin:_BlendBlendDistance,varname:node_2105,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.6824926,max:5;n:type:ShaderForge.SFN_DepthBlend,id:1711,x:31137,y:32932,varname:node_1711,prsc:2|DIST-2105-OUT;n:type:ShaderForge.SFN_Multiply,id:4215,x:32477,y:32700,varname:node_4215,prsc:2|A-9021-OUT,B-6713-OUT;n:type:ShaderForge.SFN_Multiply,id:3834,x:32547,y:33146,varname:node_3834,prsc:2|A-208-OUT,B-6713-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:6713,x:31357,y:32907,ptovrint:False,ptlb:[Blend] Enable This?,ptin:_BlendEnableThis,varname:node_6713,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-2789-OUT,B-1711-OUT;n:type:ShaderForge.SFN_Vector1,id:2789,x:31137,y:32837,varname:node_2789,prsc:2,v1:1;n:type:ShaderForge.SFN_Slider,id:8161,x:29817,y:33674,ptovrint:False,ptlb:[Primary] Height Strength,ptin:_PrimaryHeightStrength,varname:node_8161,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Multiply,id:1189,x:30256,y:33475,varname:node_1189,prsc:2|A-1720-RGB,B-8161-OUT;n:type:ShaderForge.SFN_Multiply,id:4067,x:30288,y:34360,varname:node_4067,prsc:2|A-3520-RGB,B-5319-OUT;n:type:ShaderForge.SFN_Slider,id:5319,x:29863,y:34523,ptovrint:False,ptlb:[Secondary] Height Strength,ptin:_SecondaryHeightStrength,varname:node_5319,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_FragmentPosition,id:6640,x:31126,y:33986,varname:node_6640,prsc:2;n:type:ShaderForge.SFN_ViewPosition,id:9588,x:31126,y:34111,varname:node_9588,prsc:2;n:type:ShaderForge.SFN_Distance,id:467,x:31313,y:33985,varname:node_467,prsc:2|A-6640-XYZ,B-9588-XYZ;n:type:ShaderForge.SFN_InverseLerp,id:6088,x:31584,y:33908,varname:node_6088,prsc:2|A-6358-OUT,B-5427-OUT,V-467-OUT;n:type:ShaderForge.SFN_Vector1,id:5427,x:31328,y:33942,varname:node_5427,prsc:2,v1:0;n:type:ShaderForge.SFN_Clamp01,id:3517,x:31778,y:33924,varname:node_3517,prsc:2|IN-6088-OUT;n:type:ShaderForge.SFN_Slider,id:6358,x:31035,y:33901,ptovrint:False,ptlb:[SSRefraction] Decay distance,ptin:_SSRefractionDecaydistance,varname:node_6358,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:50,max:100;n:type:ShaderForge.SFN_Fresnel,id:9434,x:31545,y:33555,varname:node_9434,prsc:2|EXP-7270-OUT;n:type:ShaderForge.SFN_Slider,id:7270,x:31155,y:33700,ptovrint:False,ptlb:[SSRefraction] Refraction Fresnel,ptin:_SSRefractionRefractionFresnel,varname:node_7270,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:5,max:5;n:type:ShaderForge.SFN_NormalVector,id:2699,x:31528,y:33700,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:9578,x:32170,y:33669,varname:node_9578,prsc:2|A-9434-OUT,B-2699-OUT,C-1205-OUT,D-3517-OUT,E-8825-OUT;n:type:ShaderForge.SFN_Transform,id:3481,x:32014,y:33400,varname:node_3481,prsc:2,tffrom:0,tfto:3|IN-9578-OUT;n:type:ShaderForge.SFN_ComponentMask,id:208,x:32170,y:33400,varname:node_208,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-3481-XYZ;n:type:ShaderForge.SFN_Vector1,id:8825,x:31757,y:34107,varname:node_8825,prsc:2,v1:-1;n:type:ShaderForge.SFN_Slider,id:1205,x:31718,y:33785,ptovrint:False,ptlb:[SSRefraction] Strength,ptin:_SSRefractionStrength,varname:node_1205,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Code,id:303,x:29836,y:31996,varname:node_303,prsc:2,code:LwAvACAARwBsAG8AcwBzAGkAbgBlAHMAcwAKAGgAYQBsAGYAIABtAGkAcAAgAD0AIAAoADEALgAwACAALQAgAEcAbABvAHMAcwBpAG4AZQBzAHMAKQAgACoAIABVAE4ASQBUAFkAXwBTAFAARQBDAEMAVQBCAEUAXwBMAE8ARABfAFMAVABFAFAAUwA7AAoALwAvACAAcwBhAG0AcABsAGUAIAB0AGgAZQAgAGQAZQBmAGEAdQBsAHQAIAByAGUAZgBsAGUAYwB0AGkAbwBuACAAYwB1AGIAZQBtAGEAcAAsACAAdQBzAGkAbgBnACAAdABoAGUAIAByAGUAZgBsAGUAYwB0AGkAbwBuACAAdgBlAGMAdABvAHIACgBoAGEAbABmADQAIABzAGsAeQBEAGEAdABhACAAPQAgAFUATgBJAFQAWQBfAFMAQQBNAFAATABFAF8AVABFAFgAQwBVAEIARQBfAEwATwBEACgAdQBuAGkAdAB5AF8AUwBwAGUAYwBDAHUAYgBlADAALAAgAEQAaQByACwAIABtAGkAcAApADsACgAvAC8AIABkAGUAYwBvAGQAZQAgAGMAdQBiAGUAbQBhAHAAIABkAGEAdABhACAAaQBuAHQAbwAgAGEAYwB0AHUAYQBsACAAYwBvAGwAbwByAAoAaABhAGwAZgAzACAAcwBrAHkAQwBvAGwAbwByACAAPQAgAEQAZQBjAG8AZABlAEgARABSACAAKABzAGsAeQBEAGEAdABhACwAIAB1AG4AaQB0AHkAXwBTAHAAZQBjAEMAdQBiAGUAMABfAEgARABSACkAOwAKAHIAZQB0AHUAcgBuACAAcwBrAHkAQwBvAGwAbwByADsA,output:2,fname:EnvironmentReflection,width:640,height:153,input:2,input:0,input_1_label:Dir,input_2_label:Glossiness|A-9458-OUT,B-9766-OUT;n:type:ShaderForge.SFN_ViewReflectionVector,id:9458,x:29336,y:31936,varname:node_9458,prsc:2;n:type:ShaderForge.SFN_SwitchProperty,id:6819,x:30409,y:32354,ptovrint:False,ptlb:[Cubemap] Use Custom Cubemap,ptin:_CubemapUseCustomCubemap,varname:node_6819,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-303-OUT,B-8115-OUT;n:type:ShaderForge.SFN_Slider,id:9766,x:29112,y:32140,ptovrint:False,ptlb:[Cubemap] Glossiness,ptin:_CubemapGlossiness,varname:node_9766,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Color,id:928,x:30906,y:32469,ptovrint:False,ptlb:[Blend] Color,ptin:_BlendColor,varname:node_928,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.6442697,c3:0.7720588,c4:1;n:type:ShaderForge.SFN_Multiply,id:9827,x:31377,y:32526,varname:node_9827,prsc:2|A-928-RGB,B-928-A;n:type:ShaderForge.SFN_Slider,id:8587,x:30749,y:32650,ptovrint:False,ptlb:[Blend] Blend Alpha,ptin:_BlendBlendAlpha,varname:node_8587,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5400593,max:1;n:type:ShaderForge.SFN_Lerp,id:9182,x:32427,y:32998,varname:node_9182,prsc:2|A-8587-OUT,B-1274-OUT,T-6713-OUT;n:type:ShaderForge.SFN_Lerp,id:1948,x:32427,y:32879,varname:node_1948,prsc:2|A-9827-OUT,B-6400-OUT,T-6713-OUT;n:type:ShaderForge.SFN_Lerp,id:9090,x:32549,y:32485,varname:node_9090,prsc:2|A-928-RGB,B-4988-OUT,T-6713-OUT;n:type:ShaderForge.SFN_Color,id:306,x:30048,y:32832,ptovrint:False,ptlb:[Cubemap] Base Color,ptin:_CubemapBaseColor,varname:node_306,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:8115,x:30219,y:32758,varname:node_8115,prsc:2|A-5108-RGB,B-306-RGB;proporder:9209-4117-4049-1720-4769-8161-4156-2710-3520-5281-5319-2398-6819-306-5108-9766-7702-3028-6713-2105-928-8587-8585-2599-2989-9021-4753-7952-3631-6358-7270-1205;pass:END;sub:END;*/

Shader "VRWaterShader/ZWrite Standard" {
    Properties {
        _BaseTexture ("[Base] Texture", 2D) = "white" {}
        _BaseColor ("[Base] Color", Color) = (0.3,0.5,0.9,0.5)
        [MaterialToggle] _BaseisasEmission ("[Base] is as Emission", Float ) = 0
        _PrimaryHeightmap ("[Primary] Heightmap", 2D) = "black" {}
        _PrimaryNormalmap ("[Primary] Normalmap", 2D) = "bump" {}
        _PrimaryHeightStrength ("[Primary] Height Strength", Range(0, 1)) = 0.5
        _PrimaryUandVspeedZWNotused ("[Primary] U and V speed (Z W Not used)", Vector) = (0.1,0.1,0,0)
        [MaterialToggle] _SecondaryEnableThis ("[Secondary] Enable This?", Float ) = 1
        _SecondaryHeightmap ("[Secondary] Heightmap", 2D) = "black" {}
        _SecondaryNormalmap ("[Secondary] Normalmap", 2D) = "bump" {}
        _SecondaryHeightStrength ("[Secondary] Height Strength", Range(0, 1)) = 0.5
        _SecondaryUandVspeedZWNotused ("[Secondary] U and V speed (Z W Not used)", Vector) = (-0.05,-0.075,0,0)
        [MaterialToggle] _CubemapUseCustomCubemap ("[Cubemap] Use Custom Cubemap", Float ) = 0
        _CubemapBaseColor ("[Cubemap] Base Color", Color) = (1,1,1,1)
        _CubemapCustomCubemap ("[Cubemap] Custom Cubemap", Cube) = "_Skybox" {}
        _CubemapGlossiness ("[Cubemap] Glossiness", Range(0, 1)) = 1
        _CubemapStrength ("[Cubemap] Strength", Range(0, 1)) = 0.4807122
        _CubemapFresnel ("[Cubemap] Fresnel", Range(0, 20)) = 4.316328
        [MaterialToggle] _BlendEnableThis ("[Blend] Enable This?", Float ) = 1
        _BlendBlendDistance ("[Blend] Blend Distance", Range(0, 5)) = 0.6824926
        _BlendColor ("[Blend] Color", Color) = (0,0.6442697,0.7720588,1)
        _BlendBlendAlpha ("[Blend] Blend Alpha", Range(0, 1)) = 0.5400593
        _TessellationStrength ("[Tessellation] Strength", Range(0, 100)) = 2.48368
        _TessellationNearCap ("[Tessellation] Near Cap", Float ) = 0
        _TessellationFarCap ("[Tessellation] Far Cap", Float ) = 20
        _OverallMetallic ("[Overall] Metallic", Range(0, 1)) = 0.9128559
        _OverallGloss ("[Overall] Gloss", Range(0, 1)) = 0.796635
        _OverallNormalStrength ("[Overall] Normal Strength", Range(0, 1)) = 0.7343698
        _OverallWaveHeight ("[Overall] Wave Height", Range(0, 30)) = 2.826293
        _SSRefractionDecaydistance ("[SSRefraction] Decay distance", Range(0, 100)) = 50
        _SSRefractionRefractionFresnel ("[SSRefraction] Refraction Fresnel", Range(0, 5)) = 5
        _SSRefractionStrength ("[SSRefraction] Strength", Range(0, 1)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 200
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcAlpha
            
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _BaseColor;
            uniform sampler2D _PrimaryHeightmap; uniform float4 _PrimaryHeightmap_ST;
            uniform sampler2D _SecondaryHeightmap; uniform float4 _SecondaryHeightmap_ST;
            uniform fixed _SecondaryEnableThis;
            uniform float4 _PrimaryUandVspeedZWNotused;
            uniform float4 _SecondaryUandVspeedZWNotused;
            uniform sampler2D _BaseTexture; uniform float4 _BaseTexture_ST;
            uniform fixed _BaseisasEmission;
            uniform float _OverallMetallic;
            uniform float _OverallGloss;
            uniform float _OverallNormalStrength;
            uniform float _OverallWaveHeight;
            uniform sampler2D _PrimaryNormalmap; uniform float4 _PrimaryNormalmap_ST;
            uniform sampler2D _SecondaryNormalmap; uniform float4 _SecondaryNormalmap_ST;
            uniform samplerCUBE _CubemapCustomCubemap;
            uniform float _CubemapStrength;
            uniform float _CubemapFresnel;
            uniform float _TessellationNearCap;
            uniform float _TessellationFarCap;
            uniform float _TessellationStrength;
            uniform float _BlendBlendDistance;
            uniform fixed _BlendEnableThis;
            uniform float _PrimaryHeightStrength;
            uniform float _SecondaryHeightStrength;
            uniform float _SSRefractionDecaydistance;
            uniform float _SSRefractionRefractionFresnel;
            uniform float _SSRefractionStrength;
            float3 EnvironmentReflection( float3 Dir , float Glossiness ){
            // Glossiness
            half mip = (1.0 - Glossiness) * UNITY_SPECCUBE_LOD_STEPS;
            // sample the default reflection cubemap, using the reflection vector
            half4 skyData = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, Dir, mip);
            // decode cubemap data into actual color
            half3 skyColor = DecodeHDR (skyData, unity_SpecCube0_HDR);
            return skyColor;
            }
            
            uniform fixed _CubemapUseCustomCubemap;
            uniform float _CubemapGlossiness;
            uniform float4 _BlendColor;
            uniform float _BlendBlendAlpha;
            uniform float4 _CubemapBaseColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 projPos : TEXCOORD7;
                UNITY_FOG_COORDS(8)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD9;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                    float2 texcoord1 : TEXCOORD1;
                    float2 texcoord2 : TEXCOORD2;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    o.texcoord1 = v.texcoord1;
                    o.texcoord2 = v.texcoord2;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float4 node_1131 = _Time;
                    float2 node_1258 = (v.texcoord0+(float2(_PrimaryUandVspeedZWNotused.r,_PrimaryUandVspeedZWNotused.g)*node_1131.g));
                    float4 _PrimaryHeightmap_var = tex2Dlod(_PrimaryHeightmap,float4(TRANSFORM_TEX(node_1258, _PrimaryHeightmap),0.0,0));
                    float2 node_4177 = (v.texcoord0+(float2(_SecondaryUandVspeedZWNotused.r,_SecondaryUandVspeedZWNotused.g)*node_1131.g));
                    float4 _SecondaryHeightmap_var = tex2Dlod(_SecondaryHeightmap,float4(TRANSFORM_TEX(node_4177, _SecondaryHeightmap),0.0,0));
                    v.vertex.xyz += (((_PrimaryHeightmap_var.rgb*_PrimaryHeightStrength)+((_SecondaryHeightmap_var.rgb*_SecondaryHeightStrength)*_SecondaryEnableThis))*v.normal*_OverallWaveHeight);
                }
                float Tessellation(TessVertex v){
                    return max(1.0,(((distance(mul(unity_ObjectToWorld, v.vertex).rgb,_WorldSpaceCameraPos)-_TessellationFarCap)/(_TessellationNearCap-_TessellationFarCap))*_TessellationStrength));
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    v.texcoord1 = vi[0].texcoord1*bary.x + vi[1].texcoord1*bary.y + vi[2].texcoord1*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_1131 = _Time;
                float2 node_1258 = (i.uv0+(float2(_PrimaryUandVspeedZWNotused.r,_PrimaryUandVspeedZWNotused.g)*node_1131.g));
                float3 _PrimaryNormalmap_var = UnpackNormal(tex2D(_PrimaryNormalmap,TRANSFORM_TEX(node_1258, _PrimaryNormalmap)));
                float2 node_4177 = (i.uv0+(float2(_SecondaryUandVspeedZWNotused.r,_SecondaryUandVspeedZWNotused.g)*node_1131.g));
                float3 _SecondaryNormalmap_var = UnpackNormal(tex2D(_SecondaryNormalmap,TRANSFORM_TEX(node_4177, _SecondaryNormalmap)));
                float3 normalLocal = float3(((_PrimaryNormalmap_var.rgb+(_SecondaryNormalmap_var.rgb*_SecondaryEnableThis)).rg*_OverallNormalStrength),1.0);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float _BlendEnableThis_var = lerp( 1.0, saturate((sceneZ-partZ)/_BlendBlendDistance), _BlendEnableThis );
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + (mul( UNITY_MATRIX_V, float4((pow(1.0-max(0,dot(normalDirection, viewDirection)),_SSRefractionRefractionFresnel)*i.normalDir*_SSRefractionStrength*saturate(((distance(i.posWorld.rgb,_WorldSpaceCameraPos)-_SSRefractionDecaydistance)/(0.0-_SSRefractionDecaydistance)))*(-1.0)),0) ).xyz.rgb.rg*_BlendEnableThis_var);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _OverallGloss;
                float perceptualRoughness = 1.0 - _OverallGloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMin[0] = unity_SpecCube0_BoxMin;
                    d.boxMin[1] = unity_SpecCube1_BoxMin;
                #endif
                #if UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMax[0] = unity_SpecCube0_BoxMax;
                    d.boxMax[1] = unity_SpecCube1_BoxMax;
                    d.probePosition[0] = unity_SpecCube0_ProbePosition;
                    d.probePosition[1] = unity_SpecCube1_ProbePosition;
                #endif
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float4 _CubemapCustomCubemap_var = texCUBElod(_CubemapCustomCubemap,float4(viewReflectDirection,_CubemapGlossiness));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = (_OverallMetallic*_BlendEnableThis_var);
                float specularMonochrome;
                float4 _BaseTexture_var = tex2D(_BaseTexture,TRANSFORM_TEX(i.uv0, _BaseTexture));
                float3 node_4988 = (_BaseColor.rgb*_BaseTexture_var.rgb);
                float3 node_9090 = lerp(_BlendColor.rgb,node_4988,_BlendEnableThis_var);
                float3 diffuseColor = node_9090; // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                half surfaceReduction;
                #ifdef UNITY_COLORSPACE_GAMMA
                    surfaceReduction = 1.0-0.28*roughness*perceptualRoughness;
                #else
                    surfaceReduction = 1.0/(roughness*roughness + 1.0);
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular + (lerp( EnvironmentReflection( viewReflectDirection , _CubemapGlossiness ), (_CubemapCustomCubemap_var.rgb*_CubemapBaseColor.rgb), _CubemapUseCustomCubemap )*_CubemapCustomCubemap_var.a*_CubemapStrength*pow(1.0-max(0,dot(normalDirection, viewDirection)),_CubemapFresnel)*8.0));
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                indirectSpecular *= surfaceReduction;
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = lerp((_BlendColor.rgb*_BlendColor.a),(node_4988*_BaseisasEmission),_BlendEnableThis_var);
/// Final Color:
                float3 finalColor = diffuse * lerp(_BlendBlendAlpha,(_BaseColor.a*_BaseTexture_var.a),_BlendEnableThis_var) + specular + emissive;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,lerp(_BlendBlendAlpha,(_BaseColor.a*_BaseTexture_var.a),_BlendEnableThis_var)),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _BaseColor;
            uniform sampler2D _PrimaryHeightmap; uniform float4 _PrimaryHeightmap_ST;
            uniform sampler2D _SecondaryHeightmap; uniform float4 _SecondaryHeightmap_ST;
            uniform fixed _SecondaryEnableThis;
            uniform float4 _PrimaryUandVspeedZWNotused;
            uniform float4 _SecondaryUandVspeedZWNotused;
            uniform sampler2D _BaseTexture; uniform float4 _BaseTexture_ST;
            uniform fixed _BaseisasEmission;
            uniform float _OverallMetallic;
            uniform float _OverallGloss;
            uniform float _OverallNormalStrength;
            uniform float _OverallWaveHeight;
            uniform sampler2D _PrimaryNormalmap; uniform float4 _PrimaryNormalmap_ST;
            uniform sampler2D _SecondaryNormalmap; uniform float4 _SecondaryNormalmap_ST;
            uniform float _TessellationNearCap;
            uniform float _TessellationFarCap;
            uniform float _TessellationStrength;
            uniform float _BlendBlendDistance;
            uniform fixed _BlendEnableThis;
            uniform float _PrimaryHeightStrength;
            uniform float _SecondaryHeightStrength;
            uniform float _SSRefractionDecaydistance;
            uniform float _SSRefractionRefractionFresnel;
            uniform float _SSRefractionStrength;
            uniform float4 _BlendColor;
            uniform float _BlendBlendAlpha;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 projPos : TEXCOORD7;
                LIGHTING_COORDS(8,9)
                UNITY_FOG_COORDS(10)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                    float2 texcoord1 : TEXCOORD1;
                    float2 texcoord2 : TEXCOORD2;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    o.texcoord1 = v.texcoord1;
                    o.texcoord2 = v.texcoord2;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float4 node_1131 = _Time;
                    float2 node_1258 = (v.texcoord0+(float2(_PrimaryUandVspeedZWNotused.r,_PrimaryUandVspeedZWNotused.g)*node_1131.g));
                    float4 _PrimaryHeightmap_var = tex2Dlod(_PrimaryHeightmap,float4(TRANSFORM_TEX(node_1258, _PrimaryHeightmap),0.0,0));
                    float2 node_4177 = (v.texcoord0+(float2(_SecondaryUandVspeedZWNotused.r,_SecondaryUandVspeedZWNotused.g)*node_1131.g));
                    float4 _SecondaryHeightmap_var = tex2Dlod(_SecondaryHeightmap,float4(TRANSFORM_TEX(node_4177, _SecondaryHeightmap),0.0,0));
                    v.vertex.xyz += (((_PrimaryHeightmap_var.rgb*_PrimaryHeightStrength)+((_SecondaryHeightmap_var.rgb*_SecondaryHeightStrength)*_SecondaryEnableThis))*v.normal*_OverallWaveHeight);
                }
                float Tessellation(TessVertex v){
                    return max(1.0,(((distance(mul(unity_ObjectToWorld, v.vertex).rgb,_WorldSpaceCameraPos)-_TessellationFarCap)/(_TessellationNearCap-_TessellationFarCap))*_TessellationStrength));
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    v.texcoord1 = vi[0].texcoord1*bary.x + vi[1].texcoord1*bary.y + vi[2].texcoord1*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_1131 = _Time;
                float2 node_1258 = (i.uv0+(float2(_PrimaryUandVspeedZWNotused.r,_PrimaryUandVspeedZWNotused.g)*node_1131.g));
                float3 _PrimaryNormalmap_var = UnpackNormal(tex2D(_PrimaryNormalmap,TRANSFORM_TEX(node_1258, _PrimaryNormalmap)));
                float2 node_4177 = (i.uv0+(float2(_SecondaryUandVspeedZWNotused.r,_SecondaryUandVspeedZWNotused.g)*node_1131.g));
                float3 _SecondaryNormalmap_var = UnpackNormal(tex2D(_SecondaryNormalmap,TRANSFORM_TEX(node_4177, _SecondaryNormalmap)));
                float3 normalLocal = float3(((_PrimaryNormalmap_var.rgb+(_SecondaryNormalmap_var.rgb*_SecondaryEnableThis)).rg*_OverallNormalStrength),1.0);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float _BlendEnableThis_var = lerp( 1.0, saturate((sceneZ-partZ)/_BlendBlendDistance), _BlendEnableThis );
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + (mul( UNITY_MATRIX_V, float4((pow(1.0-max(0,dot(normalDirection, viewDirection)),_SSRefractionRefractionFresnel)*i.normalDir*_SSRefractionStrength*saturate(((distance(i.posWorld.rgb,_WorldSpaceCameraPos)-_SSRefractionDecaydistance)/(0.0-_SSRefractionDecaydistance)))*(-1.0)),0) ).xyz.rgb.rg*_BlendEnableThis_var);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _OverallGloss;
                float perceptualRoughness = 1.0 - _OverallGloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = (_OverallMetallic*_BlendEnableThis_var);
                float specularMonochrome;
                float4 _BaseTexture_var = tex2D(_BaseTexture,TRANSFORM_TEX(i.uv0, _BaseTexture));
                float3 node_4988 = (_BaseColor.rgb*_BaseTexture_var.rgb);
                float3 node_9090 = lerp(_BlendColor.rgb,node_4988,_BlendEnableThis_var);
                float3 diffuseColor = node_9090; // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse * lerp(_BlendBlendAlpha,(_BaseColor.a*_BaseTexture_var.a),_BlendEnableThis_var) + specular;
                fixed4 finalRGBA = fixed4(finalColor,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform sampler2D _PrimaryHeightmap; uniform float4 _PrimaryHeightmap_ST;
            uniform sampler2D _SecondaryHeightmap; uniform float4 _SecondaryHeightmap_ST;
            uniform fixed _SecondaryEnableThis;
            uniform float4 _PrimaryUandVspeedZWNotused;
            uniform float4 _SecondaryUandVspeedZWNotused;
            uniform float _OverallWaveHeight;
            uniform float _TessellationNearCap;
            uniform float _TessellationFarCap;
            uniform float _TessellationStrength;
            uniform float _PrimaryHeightStrength;
            uniform float _SecondaryHeightStrength;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float2 uv1 : TEXCOORD2;
                float2 uv2 : TEXCOORD3;
                float4 posWorld : TEXCOORD4;
                float3 normalDir : TEXCOORD5;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                    float2 texcoord1 : TEXCOORD1;
                    float2 texcoord2 : TEXCOORD2;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    o.texcoord1 = v.texcoord1;
                    o.texcoord2 = v.texcoord2;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float4 node_1131 = _Time;
                    float2 node_1258 = (v.texcoord0+(float2(_PrimaryUandVspeedZWNotused.r,_PrimaryUandVspeedZWNotused.g)*node_1131.g));
                    float4 _PrimaryHeightmap_var = tex2Dlod(_PrimaryHeightmap,float4(TRANSFORM_TEX(node_1258, _PrimaryHeightmap),0.0,0));
                    float2 node_4177 = (v.texcoord0+(float2(_SecondaryUandVspeedZWNotused.r,_SecondaryUandVspeedZWNotused.g)*node_1131.g));
                    float4 _SecondaryHeightmap_var = tex2Dlod(_SecondaryHeightmap,float4(TRANSFORM_TEX(node_4177, _SecondaryHeightmap),0.0,0));
                    v.vertex.xyz += (((_PrimaryHeightmap_var.rgb*_PrimaryHeightStrength)+((_SecondaryHeightmap_var.rgb*_SecondaryHeightStrength)*_SecondaryEnableThis))*v.normal*_OverallWaveHeight);
                }
                float Tessellation(TessVertex v){
                    return max(1.0,(((distance(mul(unity_ObjectToWorld, v.vertex).rgb,_WorldSpaceCameraPos)-_TessellationFarCap)/(_TessellationNearCap-_TessellationFarCap))*_TessellationStrength));
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    v.texcoord1 = vi[0].texcoord1*bary.x + vi[1].texcoord1*bary.y + vi[2].texcoord1*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _BaseColor;
            uniform sampler2D _PrimaryHeightmap; uniform float4 _PrimaryHeightmap_ST;
            uniform sampler2D _SecondaryHeightmap; uniform float4 _SecondaryHeightmap_ST;
            uniform fixed _SecondaryEnableThis;
            uniform float4 _PrimaryUandVspeedZWNotused;
            uniform float4 _SecondaryUandVspeedZWNotused;
            uniform sampler2D _BaseTexture; uniform float4 _BaseTexture_ST;
            uniform fixed _BaseisasEmission;
            uniform float _OverallMetallic;
            uniform float _OverallGloss;
            uniform float _OverallWaveHeight;
            uniform float _TessellationNearCap;
            uniform float _TessellationFarCap;
            uniform float _TessellationStrength;
            uniform float _BlendBlendDistance;
            uniform fixed _BlendEnableThis;
            uniform float _PrimaryHeightStrength;
            uniform float _SecondaryHeightStrength;
            uniform float4 _BlendColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float4 projPos : TEXCOORD5;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                    float2 texcoord1 : TEXCOORD1;
                    float2 texcoord2 : TEXCOORD2;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    o.texcoord1 = v.texcoord1;
                    o.texcoord2 = v.texcoord2;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float4 node_1131 = _Time;
                    float2 node_1258 = (v.texcoord0+(float2(_PrimaryUandVspeedZWNotused.r,_PrimaryUandVspeedZWNotused.g)*node_1131.g));
                    float4 _PrimaryHeightmap_var = tex2Dlod(_PrimaryHeightmap,float4(TRANSFORM_TEX(node_1258, _PrimaryHeightmap),0.0,0));
                    float2 node_4177 = (v.texcoord0+(float2(_SecondaryUandVspeedZWNotused.r,_SecondaryUandVspeedZWNotused.g)*node_1131.g));
                    float4 _SecondaryHeightmap_var = tex2Dlod(_SecondaryHeightmap,float4(TRANSFORM_TEX(node_4177, _SecondaryHeightmap),0.0,0));
                    v.vertex.xyz += (((_PrimaryHeightmap_var.rgb*_PrimaryHeightStrength)+((_SecondaryHeightmap_var.rgb*_SecondaryHeightStrength)*_SecondaryEnableThis))*v.normal*_OverallWaveHeight);
                }
                float Tessellation(TessVertex v){
                    return max(1.0,(((distance(mul(unity_ObjectToWorld, v.vertex).rgb,_WorldSpaceCameraPos)-_TessellationFarCap)/(_TessellationNearCap-_TessellationFarCap))*_TessellationStrength));
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    v.texcoord1 = vi[0].texcoord1*bary.x + vi[1].texcoord1*bary.y + vi[2].texcoord1*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 _BaseTexture_var = tex2D(_BaseTexture,TRANSFORM_TEX(i.uv0, _BaseTexture));
                float3 node_4988 = (_BaseColor.rgb*_BaseTexture_var.rgb);
                float _BlendEnableThis_var = lerp( 1.0, saturate((sceneZ-partZ)/_BlendBlendDistance), _BlendEnableThis );
                o.Emission = lerp((_BlendColor.rgb*_BlendColor.a),(node_4988*_BaseisasEmission),_BlendEnableThis_var);
                
                float3 node_9090 = lerp(_BlendColor.rgb,node_4988,_BlendEnableThis_var);
                float3 diffColor = node_9090;
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, (_OverallMetallic*_BlendEnableThis_var), specColor, specularMonochrome );
                float roughness = 1.0 - _OverallGloss;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
