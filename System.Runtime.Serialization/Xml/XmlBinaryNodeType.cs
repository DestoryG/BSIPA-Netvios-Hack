using System;

namespace System.Xml
{
	// Token: 0x02000029 RID: 41
	internal enum XmlBinaryNodeType
	{
		// Token: 0x040000D9 RID: 217
		EndElement = 1,
		// Token: 0x040000DA RID: 218
		Comment,
		// Token: 0x040000DB RID: 219
		Array,
		// Token: 0x040000DC RID: 220
		MinAttribute,
		// Token: 0x040000DD RID: 221
		ShortAttribute = 4,
		// Token: 0x040000DE RID: 222
		Attribute,
		// Token: 0x040000DF RID: 223
		ShortDictionaryAttribute,
		// Token: 0x040000E0 RID: 224
		DictionaryAttribute,
		// Token: 0x040000E1 RID: 225
		ShortXmlnsAttribute,
		// Token: 0x040000E2 RID: 226
		XmlnsAttribute,
		// Token: 0x040000E3 RID: 227
		ShortDictionaryXmlnsAttribute,
		// Token: 0x040000E4 RID: 228
		DictionaryXmlnsAttribute,
		// Token: 0x040000E5 RID: 229
		PrefixDictionaryAttributeA,
		// Token: 0x040000E6 RID: 230
		PrefixDictionaryAttributeB,
		// Token: 0x040000E7 RID: 231
		PrefixDictionaryAttributeC,
		// Token: 0x040000E8 RID: 232
		PrefixDictionaryAttributeD,
		// Token: 0x040000E9 RID: 233
		PrefixDictionaryAttributeE,
		// Token: 0x040000EA RID: 234
		PrefixDictionaryAttributeF,
		// Token: 0x040000EB RID: 235
		PrefixDictionaryAttributeG,
		// Token: 0x040000EC RID: 236
		PrefixDictionaryAttributeH,
		// Token: 0x040000ED RID: 237
		PrefixDictionaryAttributeI,
		// Token: 0x040000EE RID: 238
		PrefixDictionaryAttributeJ,
		// Token: 0x040000EF RID: 239
		PrefixDictionaryAttributeK,
		// Token: 0x040000F0 RID: 240
		PrefixDictionaryAttributeL,
		// Token: 0x040000F1 RID: 241
		PrefixDictionaryAttributeM,
		// Token: 0x040000F2 RID: 242
		PrefixDictionaryAttributeN,
		// Token: 0x040000F3 RID: 243
		PrefixDictionaryAttributeO,
		// Token: 0x040000F4 RID: 244
		PrefixDictionaryAttributeP,
		// Token: 0x040000F5 RID: 245
		PrefixDictionaryAttributeQ,
		// Token: 0x040000F6 RID: 246
		PrefixDictionaryAttributeR,
		// Token: 0x040000F7 RID: 247
		PrefixDictionaryAttributeS,
		// Token: 0x040000F8 RID: 248
		PrefixDictionaryAttributeT,
		// Token: 0x040000F9 RID: 249
		PrefixDictionaryAttributeU,
		// Token: 0x040000FA RID: 250
		PrefixDictionaryAttributeV,
		// Token: 0x040000FB RID: 251
		PrefixDictionaryAttributeW,
		// Token: 0x040000FC RID: 252
		PrefixDictionaryAttributeX,
		// Token: 0x040000FD RID: 253
		PrefixDictionaryAttributeY,
		// Token: 0x040000FE RID: 254
		PrefixDictionaryAttributeZ,
		// Token: 0x040000FF RID: 255
		PrefixAttributeA,
		// Token: 0x04000100 RID: 256
		PrefixAttributeB,
		// Token: 0x04000101 RID: 257
		PrefixAttributeC,
		// Token: 0x04000102 RID: 258
		PrefixAttributeD,
		// Token: 0x04000103 RID: 259
		PrefixAttributeE,
		// Token: 0x04000104 RID: 260
		PrefixAttributeF,
		// Token: 0x04000105 RID: 261
		PrefixAttributeG,
		// Token: 0x04000106 RID: 262
		PrefixAttributeH,
		// Token: 0x04000107 RID: 263
		PrefixAttributeI,
		// Token: 0x04000108 RID: 264
		PrefixAttributeJ,
		// Token: 0x04000109 RID: 265
		PrefixAttributeK,
		// Token: 0x0400010A RID: 266
		PrefixAttributeL,
		// Token: 0x0400010B RID: 267
		PrefixAttributeM,
		// Token: 0x0400010C RID: 268
		PrefixAttributeN,
		// Token: 0x0400010D RID: 269
		PrefixAttributeO,
		// Token: 0x0400010E RID: 270
		PrefixAttributeP,
		// Token: 0x0400010F RID: 271
		PrefixAttributeQ,
		// Token: 0x04000110 RID: 272
		PrefixAttributeR,
		// Token: 0x04000111 RID: 273
		PrefixAttributeS,
		// Token: 0x04000112 RID: 274
		PrefixAttributeT,
		// Token: 0x04000113 RID: 275
		PrefixAttributeU,
		// Token: 0x04000114 RID: 276
		PrefixAttributeV,
		// Token: 0x04000115 RID: 277
		PrefixAttributeW,
		// Token: 0x04000116 RID: 278
		PrefixAttributeX,
		// Token: 0x04000117 RID: 279
		PrefixAttributeY,
		// Token: 0x04000118 RID: 280
		PrefixAttributeZ,
		// Token: 0x04000119 RID: 281
		MaxAttribute = 63,
		// Token: 0x0400011A RID: 282
		MinElement,
		// Token: 0x0400011B RID: 283
		ShortElement = 64,
		// Token: 0x0400011C RID: 284
		Element,
		// Token: 0x0400011D RID: 285
		ShortDictionaryElement,
		// Token: 0x0400011E RID: 286
		DictionaryElement,
		// Token: 0x0400011F RID: 287
		PrefixDictionaryElementA,
		// Token: 0x04000120 RID: 288
		PrefixDictionaryElementB,
		// Token: 0x04000121 RID: 289
		PrefixDictionaryElementC,
		// Token: 0x04000122 RID: 290
		PrefixDictionaryElementD,
		// Token: 0x04000123 RID: 291
		PrefixDictionaryElementE,
		// Token: 0x04000124 RID: 292
		PrefixDictionaryElementF,
		// Token: 0x04000125 RID: 293
		PrefixDictionaryElementG,
		// Token: 0x04000126 RID: 294
		PrefixDictionaryElementH,
		// Token: 0x04000127 RID: 295
		PrefixDictionaryElementI,
		// Token: 0x04000128 RID: 296
		PrefixDictionaryElementJ,
		// Token: 0x04000129 RID: 297
		PrefixDictionaryElementK,
		// Token: 0x0400012A RID: 298
		PrefixDictionaryElementL,
		// Token: 0x0400012B RID: 299
		PrefixDictionaryElementM,
		// Token: 0x0400012C RID: 300
		PrefixDictionaryElementN,
		// Token: 0x0400012D RID: 301
		PrefixDictionaryElementO,
		// Token: 0x0400012E RID: 302
		PrefixDictionaryElementP,
		// Token: 0x0400012F RID: 303
		PrefixDictionaryElementQ,
		// Token: 0x04000130 RID: 304
		PrefixDictionaryElementR,
		// Token: 0x04000131 RID: 305
		PrefixDictionaryElementS,
		// Token: 0x04000132 RID: 306
		PrefixDictionaryElementT,
		// Token: 0x04000133 RID: 307
		PrefixDictionaryElementU,
		// Token: 0x04000134 RID: 308
		PrefixDictionaryElementV,
		// Token: 0x04000135 RID: 309
		PrefixDictionaryElementW,
		// Token: 0x04000136 RID: 310
		PrefixDictionaryElementX,
		// Token: 0x04000137 RID: 311
		PrefixDictionaryElementY,
		// Token: 0x04000138 RID: 312
		PrefixDictionaryElementZ,
		// Token: 0x04000139 RID: 313
		PrefixElementA,
		// Token: 0x0400013A RID: 314
		PrefixElementB,
		// Token: 0x0400013B RID: 315
		PrefixElementC,
		// Token: 0x0400013C RID: 316
		PrefixElementD,
		// Token: 0x0400013D RID: 317
		PrefixElementE,
		// Token: 0x0400013E RID: 318
		PrefixElementF,
		// Token: 0x0400013F RID: 319
		PrefixElementG,
		// Token: 0x04000140 RID: 320
		PrefixElementH,
		// Token: 0x04000141 RID: 321
		PrefixElementI,
		// Token: 0x04000142 RID: 322
		PrefixElementJ,
		// Token: 0x04000143 RID: 323
		PrefixElementK,
		// Token: 0x04000144 RID: 324
		PrefixElementL,
		// Token: 0x04000145 RID: 325
		PrefixElementM,
		// Token: 0x04000146 RID: 326
		PrefixElementN,
		// Token: 0x04000147 RID: 327
		PrefixElementO,
		// Token: 0x04000148 RID: 328
		PrefixElementP,
		// Token: 0x04000149 RID: 329
		PrefixElementQ,
		// Token: 0x0400014A RID: 330
		PrefixElementR,
		// Token: 0x0400014B RID: 331
		PrefixElementS,
		// Token: 0x0400014C RID: 332
		PrefixElementT,
		// Token: 0x0400014D RID: 333
		PrefixElementU,
		// Token: 0x0400014E RID: 334
		PrefixElementV,
		// Token: 0x0400014F RID: 335
		PrefixElementW,
		// Token: 0x04000150 RID: 336
		PrefixElementX,
		// Token: 0x04000151 RID: 337
		PrefixElementY,
		// Token: 0x04000152 RID: 338
		PrefixElementZ,
		// Token: 0x04000153 RID: 339
		MaxElement = 119,
		// Token: 0x04000154 RID: 340
		MinText = 128,
		// Token: 0x04000155 RID: 341
		ZeroText = 128,
		// Token: 0x04000156 RID: 342
		OneText = 130,
		// Token: 0x04000157 RID: 343
		FalseText = 132,
		// Token: 0x04000158 RID: 344
		TrueText = 134,
		// Token: 0x04000159 RID: 345
		Int8Text = 136,
		// Token: 0x0400015A RID: 346
		Int16Text = 138,
		// Token: 0x0400015B RID: 347
		Int32Text = 140,
		// Token: 0x0400015C RID: 348
		Int64Text = 142,
		// Token: 0x0400015D RID: 349
		FloatText = 144,
		// Token: 0x0400015E RID: 350
		DoubleText = 146,
		// Token: 0x0400015F RID: 351
		DecimalText = 148,
		// Token: 0x04000160 RID: 352
		DateTimeText = 150,
		// Token: 0x04000161 RID: 353
		Chars8Text = 152,
		// Token: 0x04000162 RID: 354
		Chars16Text = 154,
		// Token: 0x04000163 RID: 355
		Chars32Text = 156,
		// Token: 0x04000164 RID: 356
		Bytes8Text = 158,
		// Token: 0x04000165 RID: 357
		Bytes16Text = 160,
		// Token: 0x04000166 RID: 358
		Bytes32Text = 162,
		// Token: 0x04000167 RID: 359
		StartListText = 164,
		// Token: 0x04000168 RID: 360
		EndListText = 166,
		// Token: 0x04000169 RID: 361
		EmptyText = 168,
		// Token: 0x0400016A RID: 362
		DictionaryText = 170,
		// Token: 0x0400016B RID: 363
		UniqueIdText = 172,
		// Token: 0x0400016C RID: 364
		TimeSpanText = 174,
		// Token: 0x0400016D RID: 365
		GuidText = 176,
		// Token: 0x0400016E RID: 366
		UInt64Text = 178,
		// Token: 0x0400016F RID: 367
		BoolText = 180,
		// Token: 0x04000170 RID: 368
		UnicodeChars8Text = 182,
		// Token: 0x04000171 RID: 369
		UnicodeChars16Text = 184,
		// Token: 0x04000172 RID: 370
		UnicodeChars32Text = 186,
		// Token: 0x04000173 RID: 371
		QNameDictionaryText = 188,
		// Token: 0x04000174 RID: 372
		ZeroTextWithEndElement = 129,
		// Token: 0x04000175 RID: 373
		OneTextWithEndElement = 131,
		// Token: 0x04000176 RID: 374
		FalseTextWithEndElement = 133,
		// Token: 0x04000177 RID: 375
		TrueTextWithEndElement = 135,
		// Token: 0x04000178 RID: 376
		Int8TextWithEndElement = 137,
		// Token: 0x04000179 RID: 377
		Int16TextWithEndElement = 139,
		// Token: 0x0400017A RID: 378
		Int32TextWithEndElement = 141,
		// Token: 0x0400017B RID: 379
		Int64TextWithEndElement = 143,
		// Token: 0x0400017C RID: 380
		FloatTextWithEndElement = 145,
		// Token: 0x0400017D RID: 381
		DoubleTextWithEndElement = 147,
		// Token: 0x0400017E RID: 382
		DecimalTextWithEndElement = 149,
		// Token: 0x0400017F RID: 383
		DateTimeTextWithEndElement = 151,
		// Token: 0x04000180 RID: 384
		Chars8TextWithEndElement = 153,
		// Token: 0x04000181 RID: 385
		Chars16TextWithEndElement = 155,
		// Token: 0x04000182 RID: 386
		Chars32TextWithEndElement = 157,
		// Token: 0x04000183 RID: 387
		Bytes8TextWithEndElement = 159,
		// Token: 0x04000184 RID: 388
		Bytes16TextWithEndElement = 161,
		// Token: 0x04000185 RID: 389
		Bytes32TextWithEndElement = 163,
		// Token: 0x04000186 RID: 390
		StartListTextWithEndElement = 165,
		// Token: 0x04000187 RID: 391
		EndListTextWithEndElement = 167,
		// Token: 0x04000188 RID: 392
		EmptyTextWithEndElement = 169,
		// Token: 0x04000189 RID: 393
		DictionaryTextWithEndElement = 171,
		// Token: 0x0400018A RID: 394
		UniqueIdTextWithEndElement = 173,
		// Token: 0x0400018B RID: 395
		TimeSpanTextWithEndElement = 175,
		// Token: 0x0400018C RID: 396
		GuidTextWithEndElement = 177,
		// Token: 0x0400018D RID: 397
		UInt64TextWithEndElement = 179,
		// Token: 0x0400018E RID: 398
		BoolTextWithEndElement = 181,
		// Token: 0x0400018F RID: 399
		UnicodeChars8TextWithEndElement = 183,
		// Token: 0x04000190 RID: 400
		UnicodeChars16TextWithEndElement = 185,
		// Token: 0x04000191 RID: 401
		UnicodeChars32TextWithEndElement = 187,
		// Token: 0x04000192 RID: 402
		QNameDictionaryTextWithEndElement = 189,
		// Token: 0x04000193 RID: 403
		MaxText = 189
	}
}
