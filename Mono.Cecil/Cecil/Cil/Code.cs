using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020000F3 RID: 243
	public enum Code
	{
		// Token: 0x0400042B RID: 1067
		Nop,
		// Token: 0x0400042C RID: 1068
		Break,
		// Token: 0x0400042D RID: 1069
		Ldarg_0,
		// Token: 0x0400042E RID: 1070
		Ldarg_1,
		// Token: 0x0400042F RID: 1071
		Ldarg_2,
		// Token: 0x04000430 RID: 1072
		Ldarg_3,
		// Token: 0x04000431 RID: 1073
		Ldloc_0,
		// Token: 0x04000432 RID: 1074
		Ldloc_1,
		// Token: 0x04000433 RID: 1075
		Ldloc_2,
		// Token: 0x04000434 RID: 1076
		Ldloc_3,
		// Token: 0x04000435 RID: 1077
		Stloc_0,
		// Token: 0x04000436 RID: 1078
		Stloc_1,
		// Token: 0x04000437 RID: 1079
		Stloc_2,
		// Token: 0x04000438 RID: 1080
		Stloc_3,
		// Token: 0x04000439 RID: 1081
		Ldarg_S,
		// Token: 0x0400043A RID: 1082
		Ldarga_S,
		// Token: 0x0400043B RID: 1083
		Starg_S,
		// Token: 0x0400043C RID: 1084
		Ldloc_S,
		// Token: 0x0400043D RID: 1085
		Ldloca_S,
		// Token: 0x0400043E RID: 1086
		Stloc_S,
		// Token: 0x0400043F RID: 1087
		Ldnull,
		// Token: 0x04000440 RID: 1088
		Ldc_I4_M1,
		// Token: 0x04000441 RID: 1089
		Ldc_I4_0,
		// Token: 0x04000442 RID: 1090
		Ldc_I4_1,
		// Token: 0x04000443 RID: 1091
		Ldc_I4_2,
		// Token: 0x04000444 RID: 1092
		Ldc_I4_3,
		// Token: 0x04000445 RID: 1093
		Ldc_I4_4,
		// Token: 0x04000446 RID: 1094
		Ldc_I4_5,
		// Token: 0x04000447 RID: 1095
		Ldc_I4_6,
		// Token: 0x04000448 RID: 1096
		Ldc_I4_7,
		// Token: 0x04000449 RID: 1097
		Ldc_I4_8,
		// Token: 0x0400044A RID: 1098
		Ldc_I4_S,
		// Token: 0x0400044B RID: 1099
		Ldc_I4,
		// Token: 0x0400044C RID: 1100
		Ldc_I8,
		// Token: 0x0400044D RID: 1101
		Ldc_R4,
		// Token: 0x0400044E RID: 1102
		Ldc_R8,
		// Token: 0x0400044F RID: 1103
		Dup,
		// Token: 0x04000450 RID: 1104
		Pop,
		// Token: 0x04000451 RID: 1105
		Jmp,
		// Token: 0x04000452 RID: 1106
		Call,
		// Token: 0x04000453 RID: 1107
		Calli,
		// Token: 0x04000454 RID: 1108
		Ret,
		// Token: 0x04000455 RID: 1109
		Br_S,
		// Token: 0x04000456 RID: 1110
		Brfalse_S,
		// Token: 0x04000457 RID: 1111
		Brtrue_S,
		// Token: 0x04000458 RID: 1112
		Beq_S,
		// Token: 0x04000459 RID: 1113
		Bge_S,
		// Token: 0x0400045A RID: 1114
		Bgt_S,
		// Token: 0x0400045B RID: 1115
		Ble_S,
		// Token: 0x0400045C RID: 1116
		Blt_S,
		// Token: 0x0400045D RID: 1117
		Bne_Un_S,
		// Token: 0x0400045E RID: 1118
		Bge_Un_S,
		// Token: 0x0400045F RID: 1119
		Bgt_Un_S,
		// Token: 0x04000460 RID: 1120
		Ble_Un_S,
		// Token: 0x04000461 RID: 1121
		Blt_Un_S,
		// Token: 0x04000462 RID: 1122
		Br,
		// Token: 0x04000463 RID: 1123
		Brfalse,
		// Token: 0x04000464 RID: 1124
		Brtrue,
		// Token: 0x04000465 RID: 1125
		Beq,
		// Token: 0x04000466 RID: 1126
		Bge,
		// Token: 0x04000467 RID: 1127
		Bgt,
		// Token: 0x04000468 RID: 1128
		Ble,
		// Token: 0x04000469 RID: 1129
		Blt,
		// Token: 0x0400046A RID: 1130
		Bne_Un,
		// Token: 0x0400046B RID: 1131
		Bge_Un,
		// Token: 0x0400046C RID: 1132
		Bgt_Un,
		// Token: 0x0400046D RID: 1133
		Ble_Un,
		// Token: 0x0400046E RID: 1134
		Blt_Un,
		// Token: 0x0400046F RID: 1135
		Switch,
		// Token: 0x04000470 RID: 1136
		Ldind_I1,
		// Token: 0x04000471 RID: 1137
		Ldind_U1,
		// Token: 0x04000472 RID: 1138
		Ldind_I2,
		// Token: 0x04000473 RID: 1139
		Ldind_U2,
		// Token: 0x04000474 RID: 1140
		Ldind_I4,
		// Token: 0x04000475 RID: 1141
		Ldind_U4,
		// Token: 0x04000476 RID: 1142
		Ldind_I8,
		// Token: 0x04000477 RID: 1143
		Ldind_I,
		// Token: 0x04000478 RID: 1144
		Ldind_R4,
		// Token: 0x04000479 RID: 1145
		Ldind_R8,
		// Token: 0x0400047A RID: 1146
		Ldind_Ref,
		// Token: 0x0400047B RID: 1147
		Stind_Ref,
		// Token: 0x0400047C RID: 1148
		Stind_I1,
		// Token: 0x0400047D RID: 1149
		Stind_I2,
		// Token: 0x0400047E RID: 1150
		Stind_I4,
		// Token: 0x0400047F RID: 1151
		Stind_I8,
		// Token: 0x04000480 RID: 1152
		Stind_R4,
		// Token: 0x04000481 RID: 1153
		Stind_R8,
		// Token: 0x04000482 RID: 1154
		Add,
		// Token: 0x04000483 RID: 1155
		Sub,
		// Token: 0x04000484 RID: 1156
		Mul,
		// Token: 0x04000485 RID: 1157
		Div,
		// Token: 0x04000486 RID: 1158
		Div_Un,
		// Token: 0x04000487 RID: 1159
		Rem,
		// Token: 0x04000488 RID: 1160
		Rem_Un,
		// Token: 0x04000489 RID: 1161
		And,
		// Token: 0x0400048A RID: 1162
		Or,
		// Token: 0x0400048B RID: 1163
		Xor,
		// Token: 0x0400048C RID: 1164
		Shl,
		// Token: 0x0400048D RID: 1165
		Shr,
		// Token: 0x0400048E RID: 1166
		Shr_Un,
		// Token: 0x0400048F RID: 1167
		Neg,
		// Token: 0x04000490 RID: 1168
		Not,
		// Token: 0x04000491 RID: 1169
		Conv_I1,
		// Token: 0x04000492 RID: 1170
		Conv_I2,
		// Token: 0x04000493 RID: 1171
		Conv_I4,
		// Token: 0x04000494 RID: 1172
		Conv_I8,
		// Token: 0x04000495 RID: 1173
		Conv_R4,
		// Token: 0x04000496 RID: 1174
		Conv_R8,
		// Token: 0x04000497 RID: 1175
		Conv_U4,
		// Token: 0x04000498 RID: 1176
		Conv_U8,
		// Token: 0x04000499 RID: 1177
		Callvirt,
		// Token: 0x0400049A RID: 1178
		Cpobj,
		// Token: 0x0400049B RID: 1179
		Ldobj,
		// Token: 0x0400049C RID: 1180
		Ldstr,
		// Token: 0x0400049D RID: 1181
		Newobj,
		// Token: 0x0400049E RID: 1182
		Castclass,
		// Token: 0x0400049F RID: 1183
		Isinst,
		// Token: 0x040004A0 RID: 1184
		Conv_R_Un,
		// Token: 0x040004A1 RID: 1185
		Unbox,
		// Token: 0x040004A2 RID: 1186
		Throw,
		// Token: 0x040004A3 RID: 1187
		Ldfld,
		// Token: 0x040004A4 RID: 1188
		Ldflda,
		// Token: 0x040004A5 RID: 1189
		Stfld,
		// Token: 0x040004A6 RID: 1190
		Ldsfld,
		// Token: 0x040004A7 RID: 1191
		Ldsflda,
		// Token: 0x040004A8 RID: 1192
		Stsfld,
		// Token: 0x040004A9 RID: 1193
		Stobj,
		// Token: 0x040004AA RID: 1194
		Conv_Ovf_I1_Un,
		// Token: 0x040004AB RID: 1195
		Conv_Ovf_I2_Un,
		// Token: 0x040004AC RID: 1196
		Conv_Ovf_I4_Un,
		// Token: 0x040004AD RID: 1197
		Conv_Ovf_I8_Un,
		// Token: 0x040004AE RID: 1198
		Conv_Ovf_U1_Un,
		// Token: 0x040004AF RID: 1199
		Conv_Ovf_U2_Un,
		// Token: 0x040004B0 RID: 1200
		Conv_Ovf_U4_Un,
		// Token: 0x040004B1 RID: 1201
		Conv_Ovf_U8_Un,
		// Token: 0x040004B2 RID: 1202
		Conv_Ovf_I_Un,
		// Token: 0x040004B3 RID: 1203
		Conv_Ovf_U_Un,
		// Token: 0x040004B4 RID: 1204
		Box,
		// Token: 0x040004B5 RID: 1205
		Newarr,
		// Token: 0x040004B6 RID: 1206
		Ldlen,
		// Token: 0x040004B7 RID: 1207
		Ldelema,
		// Token: 0x040004B8 RID: 1208
		Ldelem_I1,
		// Token: 0x040004B9 RID: 1209
		Ldelem_U1,
		// Token: 0x040004BA RID: 1210
		Ldelem_I2,
		// Token: 0x040004BB RID: 1211
		Ldelem_U2,
		// Token: 0x040004BC RID: 1212
		Ldelem_I4,
		// Token: 0x040004BD RID: 1213
		Ldelem_U4,
		// Token: 0x040004BE RID: 1214
		Ldelem_I8,
		// Token: 0x040004BF RID: 1215
		Ldelem_I,
		// Token: 0x040004C0 RID: 1216
		Ldelem_R4,
		// Token: 0x040004C1 RID: 1217
		Ldelem_R8,
		// Token: 0x040004C2 RID: 1218
		Ldelem_Ref,
		// Token: 0x040004C3 RID: 1219
		Stelem_I,
		// Token: 0x040004C4 RID: 1220
		Stelem_I1,
		// Token: 0x040004C5 RID: 1221
		Stelem_I2,
		// Token: 0x040004C6 RID: 1222
		Stelem_I4,
		// Token: 0x040004C7 RID: 1223
		Stelem_I8,
		// Token: 0x040004C8 RID: 1224
		Stelem_R4,
		// Token: 0x040004C9 RID: 1225
		Stelem_R8,
		// Token: 0x040004CA RID: 1226
		Stelem_Ref,
		// Token: 0x040004CB RID: 1227
		Ldelem_Any,
		// Token: 0x040004CC RID: 1228
		Stelem_Any,
		// Token: 0x040004CD RID: 1229
		Unbox_Any,
		// Token: 0x040004CE RID: 1230
		Conv_Ovf_I1,
		// Token: 0x040004CF RID: 1231
		Conv_Ovf_U1,
		// Token: 0x040004D0 RID: 1232
		Conv_Ovf_I2,
		// Token: 0x040004D1 RID: 1233
		Conv_Ovf_U2,
		// Token: 0x040004D2 RID: 1234
		Conv_Ovf_I4,
		// Token: 0x040004D3 RID: 1235
		Conv_Ovf_U4,
		// Token: 0x040004D4 RID: 1236
		Conv_Ovf_I8,
		// Token: 0x040004D5 RID: 1237
		Conv_Ovf_U8,
		// Token: 0x040004D6 RID: 1238
		Refanyval,
		// Token: 0x040004D7 RID: 1239
		Ckfinite,
		// Token: 0x040004D8 RID: 1240
		Mkrefany,
		// Token: 0x040004D9 RID: 1241
		Ldtoken,
		// Token: 0x040004DA RID: 1242
		Conv_U2,
		// Token: 0x040004DB RID: 1243
		Conv_U1,
		// Token: 0x040004DC RID: 1244
		Conv_I,
		// Token: 0x040004DD RID: 1245
		Conv_Ovf_I,
		// Token: 0x040004DE RID: 1246
		Conv_Ovf_U,
		// Token: 0x040004DF RID: 1247
		Add_Ovf,
		// Token: 0x040004E0 RID: 1248
		Add_Ovf_Un,
		// Token: 0x040004E1 RID: 1249
		Mul_Ovf,
		// Token: 0x040004E2 RID: 1250
		Mul_Ovf_Un,
		// Token: 0x040004E3 RID: 1251
		Sub_Ovf,
		// Token: 0x040004E4 RID: 1252
		Sub_Ovf_Un,
		// Token: 0x040004E5 RID: 1253
		Endfinally,
		// Token: 0x040004E6 RID: 1254
		Leave,
		// Token: 0x040004E7 RID: 1255
		Leave_S,
		// Token: 0x040004E8 RID: 1256
		Stind_I,
		// Token: 0x040004E9 RID: 1257
		Conv_U,
		// Token: 0x040004EA RID: 1258
		Arglist,
		// Token: 0x040004EB RID: 1259
		Ceq,
		// Token: 0x040004EC RID: 1260
		Cgt,
		// Token: 0x040004ED RID: 1261
		Cgt_Un,
		// Token: 0x040004EE RID: 1262
		Clt,
		// Token: 0x040004EF RID: 1263
		Clt_Un,
		// Token: 0x040004F0 RID: 1264
		Ldftn,
		// Token: 0x040004F1 RID: 1265
		Ldvirtftn,
		// Token: 0x040004F2 RID: 1266
		Ldarg,
		// Token: 0x040004F3 RID: 1267
		Ldarga,
		// Token: 0x040004F4 RID: 1268
		Starg,
		// Token: 0x040004F5 RID: 1269
		Ldloc,
		// Token: 0x040004F6 RID: 1270
		Ldloca,
		// Token: 0x040004F7 RID: 1271
		Stloc,
		// Token: 0x040004F8 RID: 1272
		Localloc,
		// Token: 0x040004F9 RID: 1273
		Endfilter,
		// Token: 0x040004FA RID: 1274
		Unaligned,
		// Token: 0x040004FB RID: 1275
		Volatile,
		// Token: 0x040004FC RID: 1276
		Tail,
		// Token: 0x040004FD RID: 1277
		Initobj,
		// Token: 0x040004FE RID: 1278
		Constrained,
		// Token: 0x040004FF RID: 1279
		Cpblk,
		// Token: 0x04000500 RID: 1280
		Initblk,
		// Token: 0x04000501 RID: 1281
		No,
		// Token: 0x04000502 RID: 1282
		Rethrow,
		// Token: 0x04000503 RID: 1283
		Sizeof,
		// Token: 0x04000504 RID: 1284
		Refanytype,
		// Token: 0x04000505 RID: 1285
		Readonly
	}
}
