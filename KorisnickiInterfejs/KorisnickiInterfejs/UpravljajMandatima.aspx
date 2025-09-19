<%@ Page Title="Upravljaj Mandatima" Language="C#" MasterPageFile="~/SednicaAdmin.Master" AutoEventWireup="true" CodeBehind="UpravljajMandatima.aspx.cs" Inherits="KorisnickiInterfejs.UpravljajMandatima" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .page-container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }
        
        .section {
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            margin-bottom: 30px;
            overflow: hidden;
        }
        
        .section-header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 20px;
            margin: 0;
        }
        
        .section-content {
            padding: 20px;
        }
        
        .form-group {
            margin-bottom: 20px;
        }
        
        .form-label {
            font-weight: bold;
            margin-bottom: 5px;
            display: block;
            color: #333;
        }
        
        .form-control {
            width: 100%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 14px;
        }
        
        .form-control:focus {
            border-color: #667eea;
            outline: none;
            box-shadow: 0 0 0 2px rgba(102,126,234,.25);
        }
        
        .btn {
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
            margin-right: 10px;
            transition: all 0.3s ease;
        }
        
        .btn-primary {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
        }
        
        .btn-primary:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 15px rgba(102,126,234,0.4);
        }
        
        .btn-secondary {
            background-color: #6c757d;
            color: white;
        }
        
        .btn-secondary:hover {
            background-color: #545b62;
        }
        
        .btn-success {
            background-color: #28a745;
            color: white;
        }
        
        .btn-success:hover {
            background-color: #218838;
        }
        
        .alert {
            padding: 15px;
            margin-bottom: 20px;
            border: 1px solid transparent;
            border-radius: 4px;
        }
        
        .alert-success {
            color: #155724;
            background-color: #d4edda;
            border-color: #c3e6cb;
        }
        
        .alert-danger {
            color: #721c24;
            background-color: #f8d7da;
            border-color: #f5c6cb;
        }
        
        .alert-warning {
            color: #856404;
            background-color: #fff3cd;
            border-color: #ffeaa7;
        }
        
        .alert-info {
            color: #0c5460;
            background-color: #d1ecf1;
            border-color: #bee5eb;
        }
        
        .grid-container {
            margin-top: 20px;
        }
        
        .gridview {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
        }
        
        .gridview th {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 12px;
            text-align: left;
            font-weight: 600;
        }
        
        .gridview td {
            padding: 10px;
            border-bottom: 1px solid #eee;
        }
        
        .gridview tr:nth-child(even) {
            background-color: #f8f9fa;
        }
        
        .gridview tr:hover {
            background-color: #e9ecef;
        }
        
        .info-box {
            background-color: #e7f3ff;
            padding: 15px;
            border-radius: 4px;
            margin-bottom: 20px;
            border-left: 4px solid #667eea;
        }
        
        .info-box h5 {
            color: #667eea;
            margin-bottom: 10px;
        }
        
        .form-row {
            display: flex;
            gap: 15px;
            margin-bottom: 15px;
        }
        
        .form-col {
            flex: 1;
        }
        
        .required-field {
            color: #dc3545;
        }
        
        .status-active {
            color: #28a745;
            font-weight: bold;
        }
        
        .status-inactive {
            color: #dc3545;
            font-weight: bold;
        }
        
        .help-text {
            background-color: #f8f9fa;
            padding: 15px;
            border-radius: 4px;
            margin-bottom: 20px;
            border-left: 4px solid #17a2b8;
        }
        
        .help-text h5 {
            color: #17a2b8;
            margin-bottom: 10px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-container">
        <h2>👥 Upravljaj Mandatima</h2>
        
        <!-- Alert Panel -->
        <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert">
            <asp:Label ID="lblAlertMessage" runat="server"></asp:Label>
        </asp:Panel>
        
        <!-- Current Saziv Info -->
        <div class="info-box">
            <h5>📋 Informacije o trenutnom sazivu</h5>
            <p><strong>Aktivan saziv:</strong> <asp:Label ID="lblAktivanSaziv" runat="server" Text="Nema aktivnog saziva"></asp:Label></p>
            <p><strong>Period:</strong> <asp:Label ID="lblPeriodSaziva" runat="server" Text="-"></asp:Label></p>
            <p><strong>Broj aktivnih mandata:</strong> <asp:Label ID="lblBrojMandata" runat="server" Text="0"></asp:Label></p>
        </div>
        
        <!-- Help Text -->
        <div class="help-text">
            <h5>💡 Kako funkcioniše upravljanje mandatima?</h5>
            <p>
                <strong>Dodavanje novih lica:</strong> Možete dodati nova lica u sistem. Lica se ne mogu brisati zbog istorijskih podataka.<br>
                <strong>Upravljanje mandatima:</strong> Povežite lica sa mandatima za trenutni saziv. Samo lica sa mandatima mogu glasati na sednicama.<br>
                <strong>Novi saziv:</strong> Kada se kreira novi saziv, potrebno je ponovo povezati lica sa mandatima za novi saziv.
            </p>
        </div>
        
        <!-- Section 1: Add New People -->
        <div class="section">
            <h3 class="section-header">➕ Dodaj Novo Lice</h3>
            <div class="section-content">
                <div class="form-row">
                    <div class="form-col">
                        <label class="form-label">
                            Ime <span class="required-field">*</span>
                        </label>
                        <asp:TextBox ID="txtIme" runat="server" CssClass="form-control" 
                                   placeholder="Unesite ime" />
                        <asp:RequiredFieldValidator ID="rfvIme" runat="server" 
                                                  ControlToValidate="txtIme" 
                                                  ErrorMessage="Ime je obavezno!" 
                                                  Display="Dynamic" CssClass="text-danger" 
                                                  ValidationGroup="DodajLice" />
                    </div>
                    <div class="form-col">
                        <label class="form-label">
                            Prezime <span class="required-field">*</span>
                        </label>
                        <asp:TextBox ID="txtPrezime" runat="server" CssClass="form-control" 
                                   placeholder="Unesite prezime" />
                        <asp:RequiredFieldValidator ID="rfvPrezime" runat="server" 
                                                  ControlToValidate="txtPrezime" 
                                                  ErrorMessage="Prezime je obavezno!" 
                                                  Display="Dynamic" CssClass="text-danger" 
                                                  ValidationGroup="DodajLice" />
                    </div>
                </div>
                
                <div class="form-row">
                    <div class="form-col">
                        <label class="form-label">
                            Korisničko ime <span class="required-field">*</span>
                        </label>
                        <asp:TextBox ID="txtKorisnickoIme" runat="server" CssClass="form-control" 
                                   placeholder="Unesite korisničko ime" />
                        <asp:RequiredFieldValidator ID="rfvKorisnickoIme" runat="server" 
                                                  ControlToValidate="txtKorisnickoIme" 
                                                  ErrorMessage="Korisničko ime je obavezno!" 
                                                  Display="Dynamic" CssClass="text-danger" 
                                                  ValidationGroup="DodajLice" />
                    </div>
                    <div class="form-col">
                        <label class="form-label">
                            Lozinka <span class="required-field">*</span>
                        </label>
                        <asp:TextBox ID="txtLozinka" runat="server" CssClass="form-control" 
                                   TextMode="Password" placeholder="Unesite lozinku (min 6 karaktera)" />
                        <asp:RequiredFieldValidator ID="rfvLozinka" runat="server" 
                                                  ControlToValidate="txtLozinka" 
                                                  ErrorMessage="Lozinka je obavezna!" 
                                                  Display="Dynamic" CssClass="text-danger" 
                                                  ValidationGroup="DodajLice" />
                    </div>
                </div>
                
                <div class="form-row">
                    <div class="form-col">
                        <label class="form-label">
                            Pozicija <span class="required-field">*</span>
                        </label>
                        <asp:DropDownList ID="ddlPozicija" runat="server" CssClass="form-control">
                            <asp:ListItem Text="-- Izaberite poziciju --" Value="" />
                            <asp:ListItem Text="Poslanik" Value="1" />
                            <asp:ListItem Text="Predsednik" Value="2" />
                            <asp:ListItem Text="Potpredsednik" Value="3" />
                            <asp:ListItem Text="Poslanik" Value="4" />
                            <asp:ListItem Text="Poslanik" Value="5" />
                            <asp:ListItem Text="Poslanik" Value="6" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvPozicija" runat="server" 
                                                  ControlToValidate="ddlPozicija" 
                                                  ErrorMessage="Pozicija je obavezna!" 
                                                  Display="Dynamic" CssClass="text-danger" 
                                                  ValidationGroup="DodajLice" />
                    </div>
                    <div class="form-col">
                        <label class="form-label">
                            Stranka <span class="required-field">*</span>
                        </label>
                        <asp:DropDownList ID="ddlStranka" runat="server" CssClass="form-control">
                            <asp:ListItem Text="-- Izaberite stranku --" Value="" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvStranka" runat="server" 
                                                  ControlToValidate="ddlStranka" 
                                                  ErrorMessage="Stranka je obavezna!" 
                                                  Display="Dynamic" CssClass="text-danger" 
                                                  ValidationGroup="DodajLice" />
                    </div>
                </div>
                
                <div class="form-row">
                    <div class="form-col">
                        <label class="form-label">
                            Datum rođenja <span class="required-field">*</span>
                        </label>
                        <asp:TextBox ID="txtDatumRodjenja" runat="server" CssClass="form-control" 
                                   TextMode="Date" />
                        <asp:RequiredFieldValidator ID="rfvDatumRodjenja" runat="server" 
                                                  ControlToValidate="txtDatumRodjenja" 
                                                  ErrorMessage="Datum rođenja je obavezan!" 
                                                  Display="Dynamic" CssClass="text-danger" 
                                                  ValidationGroup="DodajLice" />
                    </div>
                    <div class="form-col">
                        <label class="form-label">
                            Pol <span class="required-field">*</span>
                        </label>
                        <asp:DropDownList ID="ddlPol" runat="server" CssClass="form-control">
                            <asp:ListItem Text="-- Izaberite pol --" Value="" />
                            <asp:ListItem Text="Muški" Value="M" />
                            <asp:ListItem Text="Ženski" Value="Ž" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvPol" runat="server" 
                                                  ControlToValidate="ddlPol" 
                                                  ErrorMessage="Pol je obavezan!" 
                                                  Display="Dynamic" CssClass="text-danger" 
                                                  ValidationGroup="DodajLice" />
                    </div>
                </div>
                
                <div class="form-group">
                    <label class="form-label">
                        Biografija
                    </label>
                    <asp:TextBox ID="txtBiografija" runat="server" CssClass="form-control" 
                               TextMode="MultiLine" Rows="3" 
                               placeholder="Unesite kratku biografiju ili napomene..." />
                </div>
                
                <div class="form-actions">
                    <asp:Button ID="btnDodajLice" runat="server" Text="➕ Dodaj Lice" 
                               CssClass="btn btn-primary" OnClick="btnDodajLice_Click" 
                               ValidationGroup="DodajLice" />
                    <asp:Button ID="btnResetujLice" runat="server" Text="🔄 Resetuj" 
                               CssClass="btn btn-secondary" OnClick="btnResetujLice_Click" />
                </div>
            </div>
        </div>
        
        <!-- Section 2: Manage Mandates -->
        <div class="section">
            <h3 class="section-header">🎯 Upravljaj Mandatima za Trenutni Saziv</h3>
            <div class="section-content">
                <div class="form-row">
                    <div class="form-col">
                        <label class="form-label">
                            Izaberi lice <span class="required-field">*</span>
                        </label>
                        <asp:DropDownList ID="ddlLica" runat="server" CssClass="form-control">
                            <asp:ListItem Text="-- Izaberite lice --" Value="" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvLica" runat="server" 
                                                  ControlToValidate="ddlLica" 
                                                  ErrorMessage="Lice je obavezno!" 
                                                  Display="Dynamic" CssClass="text-danger" 
                                                  ValidationGroup="DodajMandat" />
                    </div>
                    <div class="form-col">
                        <label class="form-label">
                            Napomena
                        </label>
                        <asp:Label ID="lblNapomena" runat="server" Text="Mandat će biti kreiran za aktivan saziv sa strankom iz lica." 
                                   CssClass="form-control" Style="background-color: #f8f9fa; border: 1px solid #dee2e6;" />
                    </div>
                </div>
                
                <div class="form-actions">
                    <asp:Button ID="btnDodajMandat" runat="server" Text="🎯 Dodaj Mandat" 
                               CssClass="btn btn-success" OnClick="btnDodajMandat_Click" 
                               ValidationGroup="DodajMandat" />
                    <asp:Button ID="btnResetujMandat" runat="server" Text="🔄 Resetuj" 
                               CssClass="btn btn-secondary" OnClick="btnResetujMandat_Click" />
                </div>
                
                <!-- Current Mandates Grid -->
                <div class="grid-container">
                    <h4>Trenutni mandati za aktivan saziv:</h4>
                    <asp:GridView ID="gvMandati" runat="server" CssClass="gridview" 
                                  AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="ImeLica" HeaderText="Ime" />
                            <asp:BoundField DataField="PrezimeLica" HeaderText="Prezime" />
                            <asp:BoundField DataField="NazivPozicije" HeaderText="Pozicija" />
                            <asp:BoundField DataField="NazivStranke" HeaderText="Stranka" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        
        <!-- Section 3: All People Grid -->
        <div class="section">
            <h3 class="section-header">👥 Sva Lica u Sistemu</h3>
            <div class="section-content">
                <div class="grid-container">
                    <asp:GridView ID="gvSvaLica" runat="server" CssClass="gridview" 
                                  AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="Ime" HeaderText="Ime" />
                            <asp:BoundField DataField="Prezime" HeaderText="Prezime" />
                            <asp:BoundField DataField="KorisnickoIme" HeaderText="Korisničko ime" />
                            <asp:BoundField DataField="Pozicija" HeaderText="Pozicija" />
                            <asp:BoundField DataField="Stranka" HeaderText="Stranka" />
                            <asp:BoundField DataField="Pol" HeaderText="Pol" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
