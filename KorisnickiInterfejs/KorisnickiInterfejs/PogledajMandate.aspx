<%@ Page Title="Pogledaj Mandate" Language="C#" MasterPageFile="~/SednicaAdmin.Master" AutoEventWireup="true" CodeBehind="PogledajMandate.aspx.cs" Inherits="KorisnickiInterfejs.PogledajMandate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
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
            background: #6f42c1;
            color: white;
        }
        
        .btn-primary:hover {
            background: #5a32a3;
        }
        
        .btn-secondary {
            background: #6c757d;
            color: white;
        }
        
        .btn-secondary:hover {
            background: #545b62;
        }
        
        .btn-success {
            background: #28a745;
            color: white;
        }
        
        .btn-success:hover {
            background: #218838;
        }
        
        .btn-info {
            background: #17a2b8;
            color: white;
        }
        
        .btn-info:hover {
            background: #138496;
        }
        
        .results-section {
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            overflow: hidden;
        }
        
        .results-header {
            background: #6f42c1;
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
        
        .mandate-badge {
            padding: 4px 8px;
            border-radius: 12px;
            font-size: 12px;
            font-weight: 600;
            text-transform: uppercase;
        }
        
        .mandate-active {
            background: #d4edda;
            color: #155724;
        }
        
        .mandate-inactive {
            background: #f8d7da;
            color: #721c24;
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
        
        .stats-cards {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
            margin-bottom: 25px;
        }
        
        .stat-card {
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            text-align: center;
            border-left: 4px solid #6f42c1;
        }
        
        .stat-number {
            font-size: 2em;
            font-weight: bold;
            color: #6f42c1;
            margin-bottom: 5px;
        }
        
        .stat-label {
            color: #6c757d;
            font-size: 14px;
            text-transform: uppercase;
            letter-spacing: 1px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <h2>👥 Pogledaj Mandate</h2>
        <p>Pregled svih mandata u skupštini sa detaljnim informacijama</p>
    </div>
    
    <!-- Statistics Cards -->
    <div class="stats-cards">
        <div class="stat-card">
            <asp:Label ID="lblUkupnoMandata" runat="server" Text="0" CssClass="stat-number"></asp:Label>
            <div class="stat-label">Ukupno Mandata</div>
        </div>
        <div class="stat-card">
            <asp:Label ID="lblAktivnihMandata" runat="server" Text="0" CssClass="stat-number"></asp:Label>
            <div class="stat-label">Aktivnih Mandata</div>
        </div>
        <div class="stat-card">
            <asp:Label ID="lblPoslanika" runat="server" Text="0" CssClass="stat-number"></asp:Label>
            <div class="stat-label">Poslanika</div>
        </div>
        <div class="stat-card">
            <asp:Label ID="lblStranaka" runat="server" Text="0" CssClass="stat-number"></asp:Label>
            <div class="stat-label">Stranaka</div>
        </div>
    </div>
    
    <!-- Search Section -->
    <div class="search-section">
        <h3 style="margin-top: 0; color: #495057; margin-bottom: 20px;">🔍 Pretraga i Filtriranje</h3>
        
        <div class="search-row">
            <div class="search-field">
                <label for="txtImePrezime">Ime i prezime:</label>
                <asp:TextBox ID="txtImePrezime" runat="server" placeholder="Unesite ime i prezime..."></asp:TextBox>
            </div>
            
            <div class="search-field">
                <label for="ddlSaziv">Saziv:</label>
                <asp:DropDownList ID="ddlSaziv" runat="server">
                    <asp:ListItem Text="-- Svi sazivi --" Value="" />
                </asp:DropDownList>
            </div>
            
            <div class="search-field">
                <label for="ddlPozicija">Pozicija:</label>
                <asp:DropDownList ID="ddlPozicija" runat="server">
                    <asp:ListItem Text="-- Sve pozicije --" Value="" />
                    <asp:ListItem Text="Poslanik" Value="Poslanik" />
                    <asp:ListItem Text="Predsednik" Value="Predsednik" />
                    <asp:ListItem Text="Potpredsednik" Value="Potpredsednik" />
                </asp:DropDownList>
            </div>
            
            <div class="search-field">
                <label for="ddlStranka">Stranka:</label>
                <asp:DropDownList ID="ddlStranka" runat="server">
                    <asp:ListItem Text="-- Sve stranke --" Value="" />
                </asp:DropDownList>
            </div>
            
            <div class="search-buttons">
                <asp:Button ID="btnPretrazi" runat="server" Text="🔍 Pretraži" CssClass="btn btn-primary" OnClick="btnPretrazi_Click" />
                <asp:Button ID="btnResetuj" runat="server" Text="🔄 Resetuj" CssClass="btn btn-secondary" OnClick="btnResetuj_Click" />
                <asp:Button ID="btnExportuj" runat="server" Text="📊 Excel" CssClass="btn btn-success" OnClick="btnExportuj_Click" />
                <asp:Button ID="btnStampa" runat="server" Text="🖨️ Stampa" CssClass="btn btn-info" OnClick="btnStampa_Click" />
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
            <asp:GridView ID="gvMandate" runat="server" 
                         CssClass="gridview" 
                         AutoGenerateColumns="False"
                         AllowPaging="True" 
                         PageSize="15"
                         OnPageIndexChanging="gvMandate_PageIndexChanging"
                         EmptyDataText="Nema podataka za prikaz"
                         EmptyDataRowStyle-CssClass="no-data">
                
                <Columns>
                    <asp:BoundField DataField="ImePrezime" HeaderText="Ime i Prezime" />
                    <asp:BoundField DataField="NazivPozicije" HeaderText="Pozicija" />
                    <asp:BoundField DataField="NazivStranke" HeaderText="Stranka" />
                    <asp:BoundField DataField="NazivSaziva" HeaderText="Saziv" />

                    <asp:BoundField DataField="DatumMandata" HeaderText="Datum Mandata" DataFormatString="{0:dd.MM.yyyy}" />
                </Columns>
                
                <PagerStyle CssClass="pagination" />
            </asp:GridView>
        </div>
    </div>
    
    <!-- Additional Info -->
    <div style="margin-top: 20px; padding: 15px; background: #f3f0ff; border-radius: 8px; border-left: 4px solid #6f42c1;">
        <h4 style="margin: 0 0 10px 0; color: #5a32a3;">ℹ️ Informacije</h4>
        <p style="margin: 0; color: #5a32a3; font-size: 14px;">
            Ova stranica prikazuje sve mandate u skupštini. Možete koristiti filtere za pretragu po imenu, 
                            sazivu, poziciji ili stranci. Kliknite na "Pretraži" za primenu filtera ili "Resetuj" za uklanjanje svih filtera.
        </p>
    </div>
</asp:Content>
