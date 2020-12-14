<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:ms="urn:schemas-microsoft-com:xslt" 
xmlns:input="urn:external custom-input-variables"
xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
xmlns:user="http://schemas.microsoft.com/BizTalk/2003/user" 
xmlns:var="http://schemas.microsoft.com/BizTalk/2003/var" 
version="1.0">
<xsl:param name="input:XsltInput"></xsl:param>
<xsl:template match="/">
      <xsl:apply-templates select="/BAIMessage" />
   </xsl:template>
   <xsl:template match="/BAIMessage">
      <PriorDayData>
         <Header>
            <xsl:for-each select=".">
               <HeaderData>
                  <xsl:value-of select="'AsOfDate,Currency,BankIDType,BankID,Account,DataType,BAI Code,Description,Amount,Balance/Value Date,Customer Reference,Immediate Availability,1 Day Float,2+ DayFloat,Bank Reference,Text'" />
               </HeaderData>
            </xsl:for-each>
         </Header>
         <xsl:for-each select="BAIGroup">
            <xsl:for-each select="BAIAccount/Transactions">
               <Details>
                 <AsOfDate>
									<xsl:value-of select="user:DateFormat(string(../../BAIGroupHeader/AsOfDate/text()))" />
								</AsOfDate>
                  <Currency>
                     <xsl:value-of select="BAIGroup/AccountHeader/RecordAccIdentifier/CurrencyCode" />
                  </Currency>
                  <xsl:if test="string-length(../../BAIGroupHeader/OriginatorID)=9">
                     <BankIDType>
                        <xsl:value-of select="'ABA'" />
                     </BankIDType>
                  </xsl:if>
                  <xsl:if test="string-length(../../BAIGroupHeader/OriginatorID)= 8">
                     <BankIDType>
                        <xsl:value-of select="../../BAIGroupHeader/OriginatorID" />
                     </BankIDType>
                  </xsl:if>
 <xsl:if test="string-length(../../BAIGroupHeader/OriginatorID)= 11">
                     <BankIDType>
                        <xsl:value-of select="../../BAIGroupHeader/OriginatorID" />
                     </BankIDType>
                  </xsl:if>
                  <BankID>
                     <xsl:value-of select="../../BAIGroupHeader/OriginatorID" />
                  </BankID>
                  <Account>
                     <xsl:value-of select="../AccountHeader/RecordAccIdentifier/AccountNumber" />
                  </Account>

				                  <xsl:variable name="baivalue" select="TransactionRecord/BAIDetailCode" />
                  <xsl:variable name="var:DataType" select= "user:DataType($input:XsltInput,$baivalue)" />
								<xsl:if test="string($var:DataType) = 'CR'">
									<DataType>
										<xsl:value-of select="'Credits'" />
									</DataType>
								</xsl:if>
								<xsl:if test="string($var:DataType) = 'DB'">
									<DataType>
										<xsl:value-of select="'Debits'" />
									</DataType>
								</xsl:if>
								<xsl:if test="string($var:DataType) != 'CR' and string($var:DataType) != 'DB'">
									<DataType>										
​
    
</DataType>
								</xsl:if>
                  <BAICode>
                     <xsl:value-of select="TransactionRecord/BAIDetailCode" />
                  </BAICode>
				  <xsl:variable name="baidetailvalue" select="TransactionRecord/BAIDetailCode" />
                <xsl:variable name="var:Description" select="user:Description($input:XsltInput,$baidetailvalue)" />
								<Description>
									<xsl:value-of select="$var:Description" />
								</Description>
                  <Amount>
                     <xsl:value-of select="(TransactionRecord/AmountInCents) div 100" />
                  </Amount>
                  <BalanceOrValueDate>
                     <xsl:value-of select="TransactionRecord/BAIDetailCode" />
                  </BalanceOrValueDate>
                  <CustRef>
                     <xsl:value-of select="TransactionRecord/CustomerRefNo" />
                  </CustRef>
                  <BankRef>
                     <xsl:value-of select="TransactionRecord/BankRefNo" />
                  </BankRef>
                  <Text>
                     <xsl:value-of select="Continued/Detail" />
                  </Text>
               </Details>
            </xsl:for-each>
         </xsl:for-each>
      </PriorDayData>
   </xsl:template>
   <msxsl:script language="C#" implements-prefix="user">
<![CDATA[
 public string RequiredString(string input,int initialValue, int innerValue, string value)
        {
            string[] initialArray = input.Split(new string[] { "@@" }, StringSplitOptions.None);
            string Value = string.Empty;
            
            if (initialArray.Length > 0)
            {
                for (int i = 0; i < initialArray.Length; i++)
                {
                    string readValue = initialArray[i].ToString();
                    string[] innerArray = readValue.Split(new string[] { "##" }, StringSplitOptions.None);
                if (innerArray[0]==value)
                {
                   


                        Value = innerArray[innerValue].ToString();
                        return Value;

                    }
                }
              
                
            }
            return Value;
        }
public string DateFormat(string dateString)
		{​​​​​
			DateTime date = DateTime.ParseExact(dateString,"yyMMdd",System.Globalization.CultureInfo.CurrentCulture);
			string newDate = date.ToString("MM/dd/yyyy");
			return newDate;
		}​​​​​
	public string DataType(string input,string baivalue)
        {​​​​​    
			return RequiredString(input,0,1,baivalue);
        }​​​​​	
 public string Description(string input,string baidetailvalue)
        {

            return RequiredString(input,0, 2,baidetailvalue);

        }
 ]]>
</msxsl:script>
</xsl:stylesheet>