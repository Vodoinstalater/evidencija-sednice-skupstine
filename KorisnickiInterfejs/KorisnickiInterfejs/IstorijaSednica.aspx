<%@ Page Title="Istorija Sednica" Language="C#" MasterPageFile="~/SednicaAdmin.Master" AutoEventWireup="true" CodeBehind="IstorijaSednica.aspx.cs" Inherits="KorisnickiInterfejs.IstorijaSednica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .search-section {
            background: #f8f9fa;
            padding: 20px;
            border-radius: 8px;
            margin-bottom: 25px;
            border: 1px solid #e9ecef;
        }
        
        .search-row {
            display: flex;
            gap: 15px;
            align-items: end;
            flex-wrap: wrap;
        }
        
        .search-field {
            flex: 1;
            min-width: 200px;
        }
        
        .search-field label {
            display: block;
            margin-bottom: 5px;
            font-weight: 600;
            color: #495057;
        }
        
        .search-field input,
        .search-field select {
            width: 100%;
            padding: 8px 12px;
            border: 1px solid #ced4da;
            border-radius: 4px;
            font-size: 14px;
        }
        
        .search-buttons {
            display: flex;
            gap: 10px;
            align-items: end;
        }
        
        .btn {
            padding: 8px 16px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
            transition: all 0.3s ease;
        }
        
        .btn-primary {
            background: #007bff;
            color: white;
        }
        
        .btn-primary:hover {
            background: #0056b3;
        }
        
        .btn-secondary {
            background: #6c757d;
            color: white;
        }
        
        .btn-secondary:hover {
            background: #545b62;
        }
        
        .results-section {
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            overflow: hidden;
        }
        
        .results-header {
            background: #343a40;
            color: white;
            padding: 15px 20px;
            font-size: 18px;
            font-weight: 600;
        }
        
        .gridview-container {
            padding: 20px;
        }
        
        .gridview {
            width: 100%;
            border-collapse: collapse;
        }
        
        .gridview th {
            background: #f8f9fa;
            padding: 12px 8px;
            text-align: left;
            border-bottom: 2px solid #dee2e6;
            font-weight: 600;
            color: #495057;
        }
        
        .gridview td {
            padding: 10px 8px;
            border-bottom: 1px solid #dee2e6;
            vertical-align: top;
        }
        
        .gridview tr:hover {
            background: #f8f9fa;
        }
        
        .status-badge {
            padding: 4px 8px;
            border-radius: 12px;
            font-size: 12px;
            font-weight: 600;
            text-transform: uppercase;
        }
        
        .status-active {
            background: #d4edda;
            color: #155724;
        }
        
        .status-completed {
            background: #cce5ff;
            color: #004085;
        }
        
        .status-scheduled {
            background: #fff3cd;
            color: #856404;
        }
        
        .no-data {
            text-align: center;
            padding: 40px;
            color: #6c757d;
            font-style: italic;
        }
        
        .pagination {
            display: flex;
            justify-content: center;
            gap: 10px;
            margin-top: 20px;
            padding: 20px;
            border-top: 1px solid #e9ecef;
        }
        
        .pagination a {
            padding: 8px 12px;
            border: 1px solid #dee2e6;
            border-radius: 4px;
            text-decoration: none;
            color: #007bff;
            transition: all 0.3s ease;
        }
        
        .pagination a:hover {
            background: #007bff;
            color: white;
            border-color: #007bff;
        }
        
        .pagination .current {
            background: #007bff;
            color: white;
            border-color: #007bff;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <h2>📚 Istorija Sednica</h2>
        <p>Pregled svih sednica skupštine sa mogućnošću pretrage i filtriranja</p>
    </div>
    
    <!-- Search Section -->
    <div class="search-section">
        <h3 style="margin-top: 0; color: #495057; margin-bottom: 20px;">🔍 Pretraga i Filtriranje</h3>
        
        <div class="search-row">
            <div class="search-field">
                <label for="ddlSaziv">Saziv:</label>
                <asp:DropDownList ID="ddlSaziv" runat="server">
                    <asp:ListItem Text="-- Svi sazivi --" Value="" />
                </asp:DropDownList>
            </div>
            
            <!-- Zasedanje filter je uklonjen jer nije potreban -->
            
            <div class="search-field">
                <label for="txtDatumOd">Datum od:</label>
                <asp:TextBox ID="txtDatumOd" runat="server" TextMode="Date"></asp:TextBox>
            </div>
            
            <div class="search-field">
                <label for="txtDatumDo">Datum do:</label>
                <asp:TextBox ID="txtDatumDo" runat="server" TextMode="Date"></asp:TextBox>
            </div>
            
            <div class="search-buttons">
                <asp:Button ID="btnPretrazi" runat="server" Text="🔍 Pretraži" CssClass="btn btn-primary" OnClick="btnPretrazi_Click" />
                <asp:Button ID="btnResetuj" runat="server" Text="🔄 Resetuj" CssClass="btn btn-secondary" OnClick="btnResetuj_Click" />
            </div>
        </div>
    </div>
    
    <!-- Results Section -->
    <div class="results-section">
        <div class="results-header">
            📊 Rezultati pretrage
            <asp:Label ID="lblBrojRezultata" runat="server" style="float: right; font-size: 14px; opacity: 0.8;"></asp:Label>
        </div>
        
        <div class="gridview-container">
            <asp:GridView ID="gvSednice" runat="server" 
                         CssClass="gridview" 
                         AutoGenerateColumns="False"
                         AllowPaging="True" 
                         PageSize="15"
                         OnPageIndexChanging="gvSednice_PageIndexChanging"
                         EmptyDataText="Nema podataka za prikaz"
                         EmptyDataRowStyle-CssClass="no-data">
                
                <Columns>
                    <asp:BoundField DataField="Naziv" HeaderText="Naziv Sednice" />
                    <asp:BoundField DataField="Datum" HeaderText="Datum" DataFormatString="{0:dd.MM.yyyy}" />
                    <asp:BoundField DataField="Opis" HeaderText="Opis" />
                    <asp:BoundField DataField="NazivSaziva" HeaderText="Saziv" />
                    <asp:BoundField DataField="NazivZasedanja" HeaderText="Zasedanje" />
                </Columns>
                
                <PagerStyle CssClass="pagination" />
            </asp:GridView>
        </div>
    </div>
    
    <!-- Additional Info -->
    <div style="margin-top: 20px; padding: 15px; background: #e3f2fd; border-radius: 8px; border-left: 4px solid #2196f3;">
        <h4 style="margin: 0 0 10px 0; color: #1976d2;">ℹ️ Informacije</h4>
        <p style="margin: 0; color: #1565c0; font-size: 14px;">
            Ova stranica prikazuje istoriju svih sednica skupštine. Možete koristiti filtere za pretragu po 
                            sazivu ili datumu. Kliknite na "Pretraži" za primenu filtera ili "Resetuj" za uklanjanje svih filtera.
        </p>
    </div>
</asp:Content>
