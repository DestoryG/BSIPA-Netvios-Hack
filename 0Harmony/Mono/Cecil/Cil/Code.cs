using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001B7 RID: 439
	internal enum Code
	{
		// Token: 0x0400068A RID: 1674
		Nop,
		// Token: 0x0400068B RID: 1675
		Break,
		// Token: 0x0400068C RID: 1676
		Ldarg_0,
		// Token: 0x0400068D RID: 1677
		Ldarg_1,
		// Token: 0x0400068E RID: 1678
		Ldarg_2,
		// Token: 0x0400068F RID: 1679
		Ldarg_3,
		// Token: 0x04000690 RID: 1680
		Ldloc_0,
		// Token: 0x04000691 RID: 1681
		Ldloc_1,
		// Token: 0x04000692 RID: 1682
		Ldloc_2,
		// Token: 0x04000693 RID: 1683
		Ldloc_3,
		// Token: 0x04000694 RID: 1684
		Stloc_0,
		// Token: 0x04000695 RID: 1685
		Stloc_1,
		// Token: 0x04000696 RID: 1686
		Stloc_2,
		// Token: 0x04000697 RID: 1687
		Stloc_3,
		// Token: 0x04000698 RID: 1688
		Ldarg_S,
		// Token: 0x04000699 RID: 1689
		Ldarga_S,
		// Token: 0x0400069A RID: 1690
		Starg_S,
		// Token: 0x0400069B RID: 1691
		Ldloc_S,
		// Token: 0x0400069C RID: 1692
		Ldloca_S,
		// Token: 0x0400069D RID: 1693
		Stloc_S,
		// Token: 0x0400069E RID: 1694
		Ldnull,
		// Token: 0x0400069F RID: 1695
		Ldc_I4_M1,
		// Token: 0x040006A0 RID: 1696
		Ldc_I4_0,
		// Token: 0x040006A1 RID: 1697
		Ldc_I4_1,
		// Token: 0x040006A2 RID: 1698
		Ldc_I4_2,
		// Token: 0x040006A3 RID: 1699
		Ldc_I4_3,
		// Token: 0x040006A4 RID: 1700
		Ldc_I4_4,
		// Token: 0x040006A5 RID: 1701
		Ldc_I4_5,
		// Token: 0x040006A6 RID: 1702
		Ldc_I4_6,
		// Token: 0x040006A7 RID: 1703
		Ldc_I4_7,
		// Token: 0x040006A8 RID: 1704
		Ldc_I4_8,
		// Token: 0x040006A9 RID: 1705
		Ldc_I4_S,
		// Token: 0x040006AA RID: 1706
		Ldc_I4,
		// Token: 0x040006AB RID: 1707
		Ldc_I8,
		// Token: 0x040006AC RID: 1708
		Ldc_R4,
		// Token: 0x040006AD RID: 1709
		Ldc_R8,
		// Token: 0x040006AE RID: 1710
		Dup,
		// Token: 0x040006AF RID: 1711
		Pop,
		// Token: 0x040006B0 RID: 1712
		Jmp,
		// Token: 0x040006B1 RID: 1713
		Call,
		// Token: 0x040006B2 RID: 1714
		Calli,
		// Token: 0x040006B3 RID: 1715
		Ret,
		// Token: 0x040006B4 RID: 1716
		Br_S,
		// Token: 0x040006B5 RID: 1717
		Brfalse_S,
		// Token: 0x040006B6 RID: 1718
		Brtrue_S,
		// Token: 0x040006B7 RID: 1719
		Beq_S,
		// Token: 0x040006B8 RID: 1720
		Bge_S,
		// Token: 0x040006B9 RID: 1721
		Bgt_S,
		// Token: 0x040006BA RID: 1722
		Ble_S,
		// Token: 0x040006BB RID: 1723
		Blt_S,
		// Token: 0x040006BC RID: 1724
		Bne_Un_S,
		// Token: 0x040006BD RID: 1725
		Bge_Un_S,
		// Token: 0x040006BE RID: 1726
		Bgt_Un_S,
		// Token: 0x040006BF RID: 1727
		Ble_Un_S,
		// Token: 0x040006C0 RID: 1728
		Blt_Un_S,
		// Token: 0x040006C1 RID: 1729
		Br,
		// Token: 0x040006C2 RID: 1730
		Brfalse,
		// Token: 0x040006C3 RID: 1731
		Brtrue,
		// Token: 0x040006C4 RID: 1732
		Beq,
		// Token: 0x040006C5 RID: 1733
		Bge,
		// Token: 0x040006C6 RID: 1734
		Bgt,
		// Token: 0x040006C7 RID: 1735
		Ble,
		// Token: 0x040006C8 RID: 1736
		Blt,
		// Token: 0x040006C9 RID: 1737
		Bne_Un,
		// Token: 0x040006CA RID: 1738
		Bge_Un,
		// Token: 0x040006CB RID: 1739
		Bgt_Un,
		// Token: 0x040006CC RID: 1740
		Ble_Un,
		// Token: 0x040006CD RID: 1741
		Blt_Un,
		// Token: 0x040006CE RID: 1742
		Switch,
		// Token: 0x040006CF RID: 1743
		Ldind_I1,
		// Token: 0x040006D0 RID: 1744
		Ldind_U1,
		// Token: 0x040006D1 RID: 1745
		Ldind_I2,
		// Token: 0x040006D2 RID: 1746
		Ldind_U2,
		// Token: 0x040006D3 RID: 1747
		Ldind_I4,
		// Token: 0x040006D4 RID: 1748
		Ldind_U4,
		// Token: 0x040006D5 RID: 1749
		Ldind_I8,
		// Token: 0x040006D6 RID: 1750
		Ldind_I,
		// Token: 0x040006D7 RID: 1751
		Ldind_R4,
		// Token: 0x040006D8 RID: 1752
		Ldind_R8,
		// Token: 0x040006D9 RID: 1753
		Ldind_Ref,
		// Token: 0x040006DA RID: 1754
		Stind_Ref,
		// Token: 0x040006DB RID: 1755
		Stind_I1,
		// Token: 0x040006DC RID: 1756
		Stind_I2,
		// Token: 0x040006DD RID: 1757
		Stind_I4,
		// Token: 0x040006DE RID: 1758
		Stind_I8,
		// Token: 0x040006DF RID: 1759
		Stind_R4,
		// Token: 0x040006E0 RID: 1760
		Stind_R8,
		// Token: 0x040006E1 RID: 1761
		Add,
		// Token: 0x040006E2 RID: 1762
		Sub,
		// Token: 0x040006E3 RID: 1763
		Mul,
		// Token: 0x040006E4 RID: 1764
		Div,
		// Token: 0x040006E5 RID: 1765
		Div_Un,
		// Token: 0x040006E6 RID: 1766
		Rem,
		// Token: 0x040006E7 RID: 1767
		Rem_Un,
		// Token: 0x040006E8 RID: 1768
		And,
		// Token: 0x040006E9 RID: 1769
		Or,
		// Token: 0x040006EA RID: 1770
		Xor,
		// Token: 0x040006EB RID: 1771
		Shl,
		// Token: 0x040006EC RID: 1772
		Shr,
		// Token: 0x040006ED RID: 1773
		Shr_Un,
		// Token: 0x040006EE RID: 1774
		Neg,
		// Token: 0x040006EF RID: 1775
		Not,
		// Token: 0x040006F0 RID: 1776
		Conv_I1,
		// Token: 0x040006F1 RID: 1777
		Conv_I2,
		// Token: 0x040006F2 RID: 1778
		Conv_I4,
		// Token: 0x040006F3 RID: 1779
		Conv_I8,
		// Token: 0x040006F4 RID: 1780
		Conv_R4,
		// Token: 0x040006F5 RID: 1781
		Conv_R8,
		// Token: 0x040006F6 RID: 1782
		Conv_U4,
		// Token: 0x040006F7 RID: 1783
		Conv_U8,
		// Token: 0x040006F8 RID: 1784
		Callvirt,
		// Token: 0x040006F9 RID: 1785
		Cpobj,
		// Token: 0x040006FA RID: 1786
		Ldobj,
		// Token: 0x040006FB RID: 1787
		Ldstr,
		// Token: 0x040006FC RID: 1788
		Newobj,
		// Token: 0x040006FD RID: 1789
		Castclass,
		// Token: 0x040006FE RID: 1790
		Isinst,
		// Token: 0x040006FF RID: 1791
		Conv_R_Un,
		// Token: 0x04000700 RID: 1792
		Unbox,
		// Token: 0x04000701 RID: 1793
		Throw,
		// Token: 0x04000702 RID: 1794
		Ldfld,
		// Token: 0x04000703 RID: 1795
		Ldflda,
		// Token: 0x04000704 RID: 1796
		Stfld,
		// Token: 0x04000705 RID: 1797
		Ldsfld,
		// Token: 0x04000706 RID: 1798
		Ldsflda,
		// Token: 0x04000707 RID: 1799
		Stsfld,
		// Token: 0x04000708 RID: 1800
		Stobj,
		// Token: 0x04000709 RID: 1801
		Conv_Ovf_I1_Un,
		// Token: 0x0400070A RID: 1802
		Conv_Ovf_I2_Un,
		// Token: 0x0400070B RID: 1803
		Conv_Ovf_I4_Un,
		// Token: 0x0400070C RID: 1804
		Conv_Ovf_I8_Un,
		// Token: 0x0400070D RID: 1805
		Conv_Ovf_U1_Un,
		// Token: 0x0400070E RID: 1806
		Conv_Ovf_U2_Un,
		// Token: 0x0400070F RID: 1807
		Conv_Ovf_U4_Un,
		// Token: 0x04000710 RID: 1808
		Conv_Ovf_U8_Un,
		// Token: 0x04000711 RID: 1809
		Conv_Ovf_I_Un,
		// Token: 0x04000712 RID: 1810
		Conv_Ovf_U_Un,
		// Token: 0x04000713 RID: 1811
		Box,
		// Token: 0x04000714 RID: 1812
		Newarr,
		// Token: 0x04000715 RID: 1813
		Ldlen,
		// Token: 0x04000716 RID: 1814
		Ldelema,
		// Token: 0x04000717 RID: 1815
		Ldelem_I1,
		// Token: 0x04000718 RID: 1816
		Ldelem_U1,
		// Token: 0x04000719 RID: 1817
		Ldelem_I2,
		// Token: 0x0400071A RID: 1818
		Ldelem_U2,
		// Token: 0x0400071B RID: 1819
		Ldelem_I4,
		// Token: 0x0400071C RID: 1820
		Ldelem_U4,
		// Token: 0x0400071D RID: 1821
		Ldelem_I8,
		// Token: 0x0400071E RID: 1822
		Ldelem_I,
		// Token: 0x0400071F RID: 1823
		Ldelem_R4,
		// Token: 0x04000720 RID: 1824
		Ldelem_R8,
		// Token: 0x04000721 RID: 1825
		Ldelem_Ref,
		// Token: 0x04000722 RID: 1826
		Stelem_I,
		// Token: 0x04000723 RID: 1827
		Stelem_I1,
		// Token: 0x04000724 RID: 1828
		Stelem_I2,
		// Token: 0x04000725 RID: 1829
		Stelem_I4,
		// Token: 0x04000726 RID: 1830
		Stelem_I8,
		// Token: 0x04000727 RID: 1831
		Stelem_R4,
		// Token: 0x04000728 RID: 1832
		Stelem_R8,
		// Token: 0x04000729 RID: 1833
		Stelem_Ref,
		// Token: 0x0400072A RID: 1834
		Ldelem_Any,
		// Token: 0x0400072B RID: 1835
		Stelem_Any,
		// Token: 0x0400072C RID: 1836
		Unbox_Any,
		// Token: 0x0400072D RID: 1837
		Conv_Ovf_I1,
		// Token: 0x0400072E RID: 1838
		Conv_Ovf_U1,
		// Token: 0x0400072F RID: 1839
		Conv_Ovf_I2,
		// Token: 0x04000730 RID: 1840
		Conv_Ovf_U2,
		// Token: 0x04000731 RID: 1841
		Conv_Ovf_I4,
		// Token: 0x04000732 RID: 1842
		Conv_Ovf_U4,
		// Token: 0x04000733 RID: 1843
		Conv_Ovf_I8,
		// Token: 0x04000734 RID: 1844
		Conv_Ovf_U8,
		// Token: 0x04000735 RID: 1845
		Refanyval,
		// Token: 0x04000736 RID: 1846
		Ckfinite,
		// Token: 0x04000737 RID: 1847
		Mkrefany,
		// Token: 0x04000738 RID: 1848
		Ldtoken,
		// Token: 0x04000739 RID: 1849
		Conv_U2,
		// Token: 0x0400073A RID: 1850
		Conv_U1,
		// Token: 0x0400073B RID: 1851
		Conv_I,
		// Token: 0x0400073C RID: 1852
		Conv_Ovf_I,
		// Token: 0x0400073D RID: 1853
		Conv_Ovf_U,
		// Token: 0x0400073E RID: 1854
		Add_Ovf,
		// Token: 0x0400073F RID: 1855
		Add_Ovf_Un,
		// Token: 0x04000740 RID: 1856
		Mul_Ovf,
		// Token: 0x04000741 RID: 1857
		Mul_Ovf_Un,
		// Token: 0x04000742 RID: 1858
		Sub_Ovf,
		// Token: 0x04000743 RID: 1859
		Sub_Ovf_Un,
		// Token: 0x04000744 RID: 1860
		Endfinally,
		// Token: 0x04000745 RID: 1861
		Leave,
		// Token: 0x04000746 RID: 1862
		Leave_S,
		// Token: 0x04000747 RID: 1863
		Stind_I,
		// Token: 0x04000748 RID: 1864
		Conv_U,
		// Token: 0x04000749 RID: 1865
		Arglist,
		// Token: 0x0400074A RID: 1866
		Ceq,
		// Token: 0x0400074B RID: 1867
		Cgt,
		// Token: 0x0400074C RID: 1868
		Cgt_Un,
		// Token: 0x0400074D RID: 1869
		Clt,
		// Token: 0x0400074E RID: 1870
		Clt_Un,
		// Token: 0x0400074F RID: 1871
		Ldftn,
		// Token: 0x04000750 RID: 1872
		Ldvirtftn,
		// Token: 0x04000751 RID: 1873
		Ldarg,
		// Token: 0x04000752 RID: 1874
		Ldarga,
		// Token: 0x04000753 RID: 1875
		Starg,
		// Token: 0x04000754 RID: 1876
		Ldloc,
		// Token: 0x04000755 RID: 1877
		Ldloca,
		// Token: 0x04000756 RID: 1878
		Stloc,
		// Token: 0x04000757 RID: 1879
		Localloc,
		// Token: 0x04000758 RID: 1880
		Endfilter,
		// Token: 0x04000759 RID: 1881
		Unaligned,
		// Token: 0x0400075A RID: 1882
		Volatile,
		// Token: 0x0400075B RID: 1883
		Tail,
		// Token: 0x0400075C RID: 1884
		Initobj,
		// Token: 0x0400075D RID: 1885
		Constrained,
		// Token: 0x0400075E RID: 1886
		Cpblk,
		// Token: 0x0400075F RID: 1887
		Initblk,
		// Token: 0x04000760 RID: 1888
		No,
		// Token: 0x04000761 RID: 1889
		Rethrow,
		// Token: 0x04000762 RID: 1890
		Sizeof,
		// Token: 0x04000763 RID: 1891
		Refanytype,
		// Token: 0x04000764 RID: 1892
		Readonly
	}
}
