<?xml version="1.0" encoding="UTF-8"?>
<!-- General Exits -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<BOM>
			<BO>
				<AdmInfo>
					<Object>60</Object>
					<Version>2</Version>
				</AdmInfo>
				<Documents>
					<row>
						<DocNum></DocNum>
						<DocType>dDocument_Items</DocType>
						<HandWritten>tNO</HandWritten>
						<DocDate><xsl:value-of select="concat(substring(DATACOLLECTION/DATA/ACTIVITYDATE,1,4),substring(DATACOLLECTION/DATA/ACTIVITYDATE,6,2),substring(DATACOLLECTION/DATA/ACTIVITYDATE,9,2))"/></DocDate>
						<DocDueDate><xsl:value-of select="concat(substring(DATACOLLECTION/DATA/ACTIVITYDATE,1,4),substring(DATACOLLECTION/DATA/ACTIVITYDATE,6,2),substring(DATACOLLECTION/DATA/ACTIVITYDATE,9,2))"/>
						</DocDueDate>
						<DocCurrency></DocCurrency>
						<DocRate></DocRate>
						<DocTotal></DocTotal>
						<Reference1></Reference1>
						<xsl:choose>
								 <xsl:when test="DATACOLLECTION/DATA/REASONCODE = NONE">
									 <Comments></Comments>
								</xsl:when>
								<xsl:otherwise>
									 <Comments><xsl:value-of select="DATACOLLECTION/DATA/REASONCODE"/></Comments>
								</xsl:otherwise>
							</xsl:choose>
						<JournalMemo>Goods Issue</JournalMemo>
						<PaymentGroupCode>-1</PaymentGroupCode>
						<DocTime></DocTime>
						<SalesPersonCode>-</SalesPersonCode>
						<TransportationCode>-</TransportationCode>
						<Confirmed>tYES</Confirmed>
						<SummeryType>dNoSummary</SummeryType>
						<ContactPersonCode>0</ContactPersonCode>
						<ShowSCN>tNO</ShowSCN>
						<Series></Series>
						<TaxDate></TaxDate>
						<PartialSupply>tYES</PartialSupply>
						<DocObjectCode>60</DocObjectCode>
						<DiscountPercent>0.000000</DiscountPercent>
						<RevisionPo>tNO</RevisionPo>
						<BlockDunning>tNO</BlockDunning>
						<Pick>tNO</Pick>
						<PaymentBlock>tNO</PaymentBlock>
						<MaximumCashDiscount>tNO</MaximumCashDiscount>
						<WareHouseUpdateType>dwh_Stock</WareHouseUpdateType>
						<Rounding>tNO</Rounding>
						<DeferredTax>tNO</DeferredTax>
						<NumberOfInstallments>1</NumberOfInstallments>
						<DocumentSubType>bod_None</DocumentSubType>
						<UseShpdGoodsAct>tNO</UseShpdGoodsAct>
						<DownPayment>0.000000</DownPayment>
						<ReserveInvoice>tNO</ReserveInvoice>
						<SequenceModel>0</SequenceModel>
						<UseCorrectionVATGroup>tNO</UseCorrectionVATGroup>
						<DownPaymentAmount>0.000000</DownPaymentAmount>
						<DownPaymentPercentage>0.000000</DownPaymentPercentage>
						<DownPaymentType>dptInvoice</DownPaymentType>
						<DownPaymentAmountSC>0.000000</DownPaymentAmountSC>
						<DownPaymentAmountFC>0.000000</DownPaymentAmountFC>
						<VatPercent>0.000000</VatPercent>
						<OpeningRemarks/>
						<ClosingRemarks/>
						<RoundingDiffAmount>0.000000</RoundingDiffAmount>
					</row>
				</Documents>
				<Document_Lines>
					<row>
						<LineNum>0</LineNum>
						<ItemCode><xsl:value-of select="DATACOLLECTION/DATA/SKU"/></ItemCode>
						<ItemDescription></ItemDescription>
						<Quantity><xsl:value-of select="DATACOLLECTION/DATA/TOQTY - DATACOLLECTION/DATA/FROMQTY"/></Quantity>
						<Price></Price>
						<PriceAfterVAT></PriceAfterVAT>
						<Currency></Currency>
						<DiscountPercent>0.000000</DiscountPercent>
						<VendorNum/>
						<xsl:choose>
								 <xsl:when test="DATACOLLECTION/DATA/TOSTATUS = AVAILABLE">
									 <WarehouseCode>01</WarehouseCode>
								</xsl:when>
								<xsl:when test="DATACOLLECTION/DATA/TOSTATUS = DAMAGE">
									 <WarehouseCode>02</WarehouseCode>
								</xsl:when>
								<xsl:otherwise>
									 <WarehouseCode>01</WarehouseCode>
								</xsl:otherwise>
							</xsl:choose>
						<SalesPersonCode>-1</SalesPersonCode>
						<CommisionPercent>0.000000</CommisionPercent>
						<TreeType>iNotATree</TreeType>
						<AccountCode></AccountCode>
						<UseBaseUnits>tYES</UseBaseUnits>
						<CostingCode/>
						<ProjectCode/>
						<BarCode/>
						<Height1>0.000000</Height1>
						<Height2>0.000000</Height2>
						<Lengh1>0.000000</Lengh1>
						<Lengh2>0.000000</Lengh2>
						<Weight1>0.000000</Weight1>
						<Weight2>0.000000</Weight2>
						<Factor1>1.000000</Factor1>
						<Factor2>1.000000</Factor2>
						<Factor3>1.000000</Factor3>
						<Factor4>1.000000</Factor4>
						<BaseType>-1</BaseType>
						<Volume>0.000000</Volume>
						<Width1>0.000000</Width1>
						<Width2>0.000000</Width2>
						<ShippingMethod>3</ShippingMethod>
						<CorrectionInvoiceItem>ciis_ShouldBe</CorrectionInvoiceItem>
						<CorrInvAmountToStock>0.000000</CorrInvAmountToStock>
						<CorrInvAmountToDiffAcct>0.000000</CorrInvAmountToDiffAcct>
						<DeferredTax>tNO</DeferredTax>
						<LineTotal>400.000000</LineTotal>
						<TaxPercentagePerRow>0.000000</TaxPercentagePerRow>
						<ExciseAmount>0.000000</ExciseAmount>
						<RowTotalFC>0.000000</RowTotalFC>
						<UnitPrice></UnitPrice>
						<LineStatus>bost_Open</LineStatus>
						<LineType>dlt_Regular</LineType>
						<ItemDetails/>
					</row>
				</Document_Lines>
			</BO>
		</BOM>
	</xsl:template>
</xsl:stylesheet>
