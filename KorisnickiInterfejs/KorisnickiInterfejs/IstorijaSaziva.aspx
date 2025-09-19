<%@ Page Title="Istorija Saziva" Language="C#" MasterPageFile="~/SednicaAdmin.Master" AutoEventWireup="true" CodeBehind="IstorijaSaziva.aspx.cs" Inherits="KorisnickiInterfejs.IstorijaSaziva" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
    <style>
        
        .results-section {
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            overflow: hidden;
        }
        
        .results-header {
            background: #17a2b8;
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
                    <h2>🏛️ Istorija Saziva</h2>
            <p>Pregled svih saziva skupštine sa detaljnim informacijama</p>
    </div>
    
    
    
    
    <!-- Results Section -->
    <div class="results-section">
        <div class="results-header">
            📊 Istorija Saziva
        </div>
        
        <div class="gridview-container">
            <asp:GridView ID="gvSazivi" runat="server" 
                         CssClass="gridview" 
                         AutoGenerateColumns="False"
                         AllowPaging="True" 
                         PageSize="15"
                         OnPageIndexChanging="gvSazivi_PageIndexChanging"
                         EmptyDataText="Nema podataka za prikaz"
                         EmptyDataRowStyle-CssClass="no-data">
                
                <Columns>
                    <asp:BoundField DataField="Naziv" HeaderText="Naziv Saziva" />
                    <asp:BoundField DataField="DatumPocetka" HeaderText="Datum Početka" DataFormatString="{0:dd.MM.yyyy}" />
                    <asp:BoundField DataField="DatumZavrsetka" HeaderText="Datum Završetka" DataFormatString="{0:dd.MM.yyyy}" />
                    <asp:BoundField DataField="Opis" HeaderText="Opis" />
                </Columns>
                
                <PagerStyle CssClass="pagination" />
            </asp:GridView>
        </div>
    </div>
    
    <!-- Additional Info -->
    <div style="margin-top: 20px; padding: 15px; background: #e3f2fd; border-radius: 8px; border-left: 4px solid #17a2b8;">
        <h4 style="margin: 0 0 10px 0; color: #0c5460;">ℹ️ Informacije</h4>
        <p style="margin: 0; color: #0c5460; font-size: 14px;">
            Ova stranica prikazuje istoriju svih saziva skupštine sa detaljnim informacijama.
        </p>
    </div>
</asp:Content>
