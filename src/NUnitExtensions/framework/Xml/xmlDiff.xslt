<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	extension-element-prefixes="msxsl"
	xmlns="urn:nunit.org:diff"
>
  <xsl:variable name="actual" select="document('file:///actual.xml')" />
  
  <xsl:template name="diff">
    <xsl:param name="expected-nodes"/>
    <xsl:param name="actual-nodes"/>
    <diff name="{name($expected-nodes)}">
      <xsl:choose>
        <xsl:when test="self::* and (local-name($expected-nodes) != local-name($actual-nodes) or namespace-uri($expected-nodes) != namespace-uri($actual-nodes))">
          <mismatch diff="names">
            <expected>
              <xsl:copy-of select="$expected-nodes"/>
            </expected>
            <actual>
              <xsl:copy-of select="$actual-nodes"/>
            </actual>
          </mismatch>
        </xsl:when>
        <xsl:when test="count($expected-nodes/@*) != count($actual-nodes/@*)">
          <mismatch diff="number of children attributes ({count($expected-nodes/@*)} versus {count($actual-nodes/@*)} )">
            <expected>
              <xsl:copy-of select="$expected-nodes"/>
            </expected>
            <actual>
              <xsl:copy-of select="$actual-nodes"/>
            </actual>
          </mismatch>
        </xsl:when>
        <xsl:when test="count($expected-nodes/*) != count($actual-nodes/*)">
          <mismatch diff="number of children elements ({count($expected-nodes/*)} versus {count($actual-nodes/*)} )">
            <expected>
              <xsl:copy-of select="$expected-nodes"/>
            </expected>
            <actual>
              <xsl:copy-of select="$actual-nodes"/>
            </actual>
          </mismatch>
        </xsl:when>
        <xsl:when test="count($expected-nodes/text()) != count($actual-nodes/text())">
          <mismatch diff="number of children text nodes ({count($expected-nodes/text())} versus {count($actual-nodes/text())} )">
            <expected>
              <xsl:copy-of select="$expected-nodes"/>
            </expected>
            <actual>
              <xsl:copy-of select="$actual-nodes"/>
            </actual>
          </mismatch>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="$expected-nodes/@*" mode="diff">
            <xsl:with-param name="actual-nodes" select="$actual-nodes"/>
          </xsl:apply-templates>
          <xsl:apply-templates select="$expected-nodes/*" mode="diff">
            <xsl:with-param name="actual-nodes" select="$actual-nodes"/>
          </xsl:apply-templates>
          <xsl:apply-templates select="$expected-nodes/text()" mode="diff">
            <xsl:with-param name="actual-nodes" select="$actual-nodes"/>
          </xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose>
    </diff>
  </xsl:template>
  
  <xsl:template match="*" mode="diff">
    <xsl:param name="pos" select="position()"/>
    <xsl:param name="actual-nodes"/>
    <xsl:param name="actual-node" select="$actual-nodes/*[position()=$pos]"/>
    <xsl:call-template name="diff">
      <xsl:with-param name="expected-nodes" select="."/>
      <xsl:with-param name="actual-nodes" select="$actual-node"/>
    </xsl:call-template>
  </xsl:template>
  
  <xsl:template match="text()" mode="diff">
    <xsl:param name="current" select="."/>
    <xsl:param name="pos" select="position()"/>
    <xsl:param name="actual-nodes"/>
    <xsl:param name="actual-node" select="$actual-nodes/text()[position()=$pos]"/>
    <xsl:if test="not(. = $actual-node)">
      <mismatch>
        <expected>
          <xsl:copy-of select="."/>
        </expected>
        <actual>
          <xsl:copy-of select="$actual-node"/>
        </actual>
      </mismatch>
    </xsl:if>
  </xsl:template>
  
  <xsl:template match="@*" mode="diff">
    <xsl:param name="current" select="."/>
    <xsl:param name="actual-nodes"/>
    <xsl:param name="actual-node" select="$actual-nodes/@*[local-name() = local-name(current()) and namespace-uri() = namespace-uri(current())]"/>
    <xsl:if test="not(. = $actual-node)">
      <mismatch>
        <expected>
          <xsl:copy-of select="."/>
        </expected>
        <actual>
          <xsl:copy-of select="$actual-node"/>
        </actual>
      </mismatch>
    </xsl:if>
  </xsl:template>

  <xsl:template match="/">
    <xsl:call-template name="diff">
      <xsl:with-param name="expected-nodes" select="." />
      <xsl:with-param name="actual-nodes" select="$actual" />
    </xsl:call-template>
  </xsl:template>
</xsl:stylesheet>
