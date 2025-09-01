<%@ Page Title="Konstituisi Novi Saziv" Language="C#" MasterPageFile="~/SednicaAdmin.Master" AutoEventWireup="true" CodeBehind="NoviSaziv.aspx.cs" Inherits="KorisnickiInterfejs.NoviSaziv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-container {
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
        }
        
        .form-group {
            margin-bottom: 20px;
        }
        
        .form-label {
            font-weight: bold;
            margin-bottom: 5px;
            display: block;
        }
        
        .form-control {
            width: 100%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 14px;
        }
        
        .form-control:focus {
            border-color: #007bff;
            outline: none;
            box-shadow: 0 0 0 2px rgba(0,123,255,.25);
        }
        
        .btn {
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
            margin-right: 10px;
        }
        
        .btn-primary {
            background-color: #007bff;
            color: white;
        }
        
        .btn-primary:hover {
            background-color: #0056b3;
        }
        
        .btn-secondary {
            background-color: #6c757d;
            color: white;
        }
        
        .btn-secondary:hover {
            background-color: #545b62;
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
        
        .help-text {
            background-color: #f8f9fa;
            padding: 20px;
            border-radius: 4px;
            margin-bottom: 30px;
        }
        
        .help-text h4 {
            color: #495057;
            margin-bottom: 15px;
        }
        
        .help-text p {
            color: #6c757d;
            line-height: 1.6;
        }
        
        .current-info {
            background-color: #e7f3ff;
            padding: 15px;
            border-radius: 4px;
            margin-bottom: 20px;
            border-left: 4px solid #007bff;
        }
        
        .current-info h5 {
            color: #0056b3;
            margin-bottom: 10px;
        }
        
        .form-actions {
            margin-top: 30px;
            padding-top: 20px;
            border-top: 1px solid #dee2e6;
        }
        
        .required-field {
            color: #dc3545;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-container">
        <h2>🏛️ Konstituisi Novi Saziv</h2>
        
        <!-- Alert Panel -->
        <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert">
            <asp:Label ID="lblAlertMessage" runat="server"></asp:Label>
        </asp:Panel>
        
        <!-- Trenutni Saziv Info -->
        <div class="current-info">
            <h5>📋 Informacije o trenutnom sazivu</h5>
            <p><strong>Trenutni saziv:</strong> <asp:Label ID="lblTrenutniSaziv" runat="server" Text="Nema aktivnog saziva"></asp:Label></p>
            <p><strong>Period:</strong> <asp:Label ID="lblPeriodSaziva" runat="server" Text="-"></asp:Label></p>
            <p><strong>Status:</strong> <asp:Label ID="lblStatusSaziva" runat="server" Text="-"></asp:Label></p>
        </div>
        
        <!-- Help Text -->
        <div class="help-text">
            <h4>💡 Kako funkcioniše konstituisanje novog saziva?</h4>
            <p>
                Kada konstituišete novi saziv, automatski se zatvara trenutno aktivan saziv (ako postoji) 
                i novi saziv postaje aktivan. Novi saziv će biti dostupan za kreiranje zasedanja i sednica. 
                Svi novi entiteti (zasedanja, sednice, mandati) će se automatski povezati sa novim sazivom.
            </p>
            <h5>⚠️ Važne napomene</h5>
            <p>
                • Datum početka ne sme biti u prošlosti<br>
                • Datum završetka mora biti nakon datuma početka<br>
                • Naziv saziva je obavezan<br>
                • Opis je opciono polje za dodatne informacije
            </p>
        </div>
        
        <!-- Form -->
        <div class="form-group">
            <label for="txtNazivSaziva" class="form-label">
                Naziv saziva <span class="required-field">*</span>
            </label>
            <asp:TextBox ID="txtNazivSaziva" runat="server" CssClass="form-control" 
                       placeholder="Unesite naziv novog saziva (npr. 'X Saziv Narodne skupštine')" />
            <asp:RequiredFieldValidator ID="rfvNazivSaziva" runat="server" 
                                      ControlToValidate="txtNazivSaziva" 
                                      ErrorMessage="Naziv saziva je obavezan!" 
                                      Display="Dynamic" CssClass="text-danger" />
        </div>
        
        <div class="form-group">
            <label for="txtDatumPocetka" class="form-label">
                Datum početka <span class="required-field">*</span>
            </label>
            <asp:TextBox ID="txtDatumPocetka" runat="server" CssClass="form-control" 
                       TextMode="Date" />
            <asp:RequiredFieldValidator ID="rfvDatumPocetka" runat="server" 
                                      ControlToValidate="txtDatumPocetka" 
                                      ErrorMessage="Datum početka je obavezan!" 
                                      Display="Dynamic" CssClass="text-danger" />
        </div>
        
        <div class="form-group">
            <label for="txtDatumZavrsetka" class="form-label">
                Datum završetka <span class="required-field">*</span>
            </label>
            <asp:TextBox ID="txtDatumZavrsetka" runat="server" CssClass="form-control" 
                       TextMode="Date" />
            <asp:RequiredFieldValidator ID="rfvDatumZavrsetka" runat="server" 
                                      ControlToValidate="txtDatumZavrsetka" 
                                      ErrorMessage="Datum završetka je obavezan!" 
                                      Display="Dynamic" CssClass="text-danger" />
        </div>
        
        <div class="form-group">
            <label for="txtOpisSaziva" class="form-label">
                Opis saziva
            </label>
            <asp:TextBox ID="txtOpisSaziva" runat="server" CssClass="form-control" 
                       TextMode="MultiLine" Rows="4" 
                       placeholder="Unesite opis saziva, napomene ili dodatne informacije..." />
        </div>
        
        <!-- Form Actions -->
        <div class="form-actions">
            <asp:Button ID="btnKonstituisiSaziv" runat="server" Text="🏛️ Konstituisi Saziv" 
                       CssClass="btn btn-primary" OnClick="btnKonstituisiSaziv_Click" />
            <asp:Button ID="btnResetuj" runat="server" Text="🔄 Resetuj" 
                       CssClass="btn btn-secondary" OnClick="btnResetuj_Click" />
        </div>
    </div>
    
    <script type="text/javascript">
        // Set default dates
        document.addEventListener('DOMContentLoaded', function() {
            const today = new Date();
            const startDateInput = document.getElementById('<%= txtDatumPocetka.ClientID %>');
            const endDateInput = document.getElementById('<%= txtDatumZavrsetka.ClientID %>');
            
            if (startDateInput) {
                startDateInput.value = today.toISOString().split('T')[0];
            }
            
            if (endDateInput) {
                // Set end date to 4 years from today (typical saziv duration)
                const fourYearsLater = new Date(today.getFullYear() + 4, today.getMonth(), today.getDate());
                endDateInput.value = fourYearsLater.toISOString().split('T')[0];
            }
        });
        
        // Client-side validation
        function validateForm() {
            const startDate = new Date(document.getElementById('<%= txtDatumPocetka.ClientID %>').value);
            const endDate = new Date(document.getElementById('<%= txtDatumZavrsetka.ClientID %>').value);
            const today = new Date();
            
            // Reset time part for comparison
            today.setHours(0, 0, 0, 0);
            startDate.setHours(0, 0, 0, 0);
            endDate.setHours(0, 0, 0, 0);
            
            if (startDate < today) {
                alert('Greška: Datum početka ne sme biti u prošlosti.');
                return false;
            }
            
            if (endDate <= startDate) {
                alert('Greška: Datum završetka mora biti nakon datuma početka.');
                return false;
            }
            
            return true;
        }
    </script>
</asp:Content>
