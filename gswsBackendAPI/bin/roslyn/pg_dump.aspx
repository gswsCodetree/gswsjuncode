<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pg_dump.aspx.cs" Inherits="TestApp.pg_dump" %>
<%=Request["msg"] %>
<script type="text/javascript">
	function getMsg() {
	  var msg = '<%=Request["msg"] %>';
	  AndroidFunction.gotMsg(msg);
	}
    getMsg();
</script>
