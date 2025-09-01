<%@ Page Title="" Language="C#" MasterPageFile="~/SednicaAdmin.Master" AutoEventWireup="true" CodeBehind="NovaSednica.aspx.cs" Inherits="KorisnickiInterfejs.NovaSednica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .content-header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 30px;
            border-radius: 15px;
            margin-bottom: 30px;
            text-align: center;
        }

        .content-header h2 {
            margin: 0 0 10px 0;
            font-size: 2.5em;
            font-weight: 300;
        }

        .content-header p {
            margin: 0;
            font-size: 1.2em;
            opacity: 0.9;
        }

        .form-container {
            background: white;
            border-radius: 15px;
            padding: 40px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.1);
            border: 1px solid #e9ecef;
            max-width: 800px;
            margin: 0 auto;
        }

        .form-group {
            margin-bottom: 25px;
        }

        .form-group label {
            display: block;
            margin-bottom: 10px;
            color: #495057;
            font-weight: 600;
            font-size: 16px;
        }

        .form-control {
            width: 100%;
            padding: 15px 20px;
            border: 2px solid #e9ecef;
            border-radius: 10px;
            font-size: 16px;
            transition: all 0.3s ease;
            box-sizing: border-box;
        }

        .form-control:focus {
            outline: none;
            border-color: #667eea;
            box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.1);
            transform: translateY(-2px);
        }

        .btn {
            padding: 15px 30px;
            border: none;
            border-radius: 10px;
            font-size: 16px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            text-decoration: none;
            display: inline-block;
            text-align: center;
            min-width: 150px;
        }

        .btn-primary {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
        }

        .btn-primary:hover {
            transform: translateY(-3px);
            box-shadow: 0 10px 30px rgba(102, 126, 234, 0.3);
        }

        .btn-secondary {
            background: #6c757d;
            color: white;
            margin-left: 15px;
        }

        .btn-secondary:hover {
            background: #5a6268;
            transform: translateY(-3px);
        }

        .btn-success {
            background: #28a745;
            color: white;
            padding: 8px 16px;
            font-size: 14px;
            min-width: auto;
        }

        .btn-success:hover {
            background: #218838;
            transform: translateY(-2px);
        }

        .btn-danger {
            background: #dc3545;
            color: white;
            border: none;
            border-radius: 50%;
            width: 30px;
            height: 30px;
            font-size: 18px;
            cursor: pointer;
            margin-left: 10px;
        }

        .btn-danger:hover {
            background: #c82333;
        }

        /* Stilovi za pravilo 7 dana */
        .alert-warning {
            background-color: #fff3cd;
            border: 1px solid #ffeaa7;
            color: #856404;
            padding: 15px;
            border-radius: 10px;
            margin-bottom: 15px;
        }

        .alert-warning h5 {
            margin: 0 0 10px 0;
            color: #856404;
        }

        .alert-warning p {
            margin: 0;
            font-size: 14px;
        }

        .form-text {
            font-size: 12px;
            color: #6c757d;
            margin-top: 5px;
        }

        .current-session-info {
            background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
            color: white;
            padding: 25px;
            border-radius: 15px;
            margin-bottom: 30px;
            text-align: center;
        }

        .current-session-info h3 {
            margin: 0 0 20px 0;
            font-size: 1.4em;
        }

        .session-details {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
        }

        .detail-item {
            background: rgba(255,255,255,0.1);
            padding: 20px;
            border-radius: 10px;
            backdrop-filter: blur(10px);
        }

        .detail-label {
            font-size: 0.9em;
            opacity: 0.8;
            margin-bottom: 8px;
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        .detail-value {
            font-size: 1.2em;
            font-weight: 600;
        }

        .questions-section {
            background: #f8f9fa;
            padding: 25px;
            border-radius: 15px;
            margin: 25px 0;
            border: 2px solid #e9ecef;
        }

        .questions-section h4 {
            color: #495057;
            margin: 0 0 20px 0;
            font-size: 1.3em;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .question-item {
            display: flex;
            gap: 15px;
            margin-bottom: 20px;
            align-items: center;
            background: white;
            padding: 20px;
            border-radius: 10px;
            border: 1px solid #e9ecef;
            box-shadow: 0 2px 5px rgba(0,0,0,0.05);
        }

        .question-item input {
            flex: 1;
            padding: 12px 15px;
            border: 2px solid #e9ecef;
            border-radius: 8px;
            font-size: 16px;
            transition: border-color 0.3s ease;
        }

        .question-item input:focus {
            outline: none;
            border-color: #667eea;
            box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
        }

        .question-number {
            background: #667eea;
            color: white;
            width: 30px;
            height: 30px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-weight: 600;
            font-size: 14px;
        }

        .alert {
            padding: 20px;
            border-radius: 10px;
            margin-bottom: 25px;
            border-left: 5px solid;
            font-weight: 500;
        }

        .alert-success {
            background: #d4edda;
            border-color: #28a745;
            color: #155724;
        }

        .alert-danger {
            background: #f8d7da;
            border-color: #dc3545;
            color: #721c24;
        }

        .alert-info {
            background: #d1ecf1;
            border-color: #17a2b8;
            color: #0c5460;
        }

        .form-actions {
            text-align: center;
            margin-top: 30px;
            padding-top: 30px;
            border-top: 2px solid #e9ecef;
        }

        .help-text {
            background: #f8f9fa;
            padding: 20px;
            border-radius: 10px;
            margin-top: 30px;
            border-left: 4px solid #17a2b8;
        }

        .help-text h4 {
            color: #17a2b8;
            margin: 0 0 15px 0;
        }

        .help-text p {
            color: #6c757d;
            margin: 0;
            line-height: 1.6;
        }

        .form-row {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 20px;
        }

        @media (max-width: 768px) {
            .form-row {
                grid-template-columns: 1fr;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-header">
        <h2>📅 Sazovi Novu Sednicu</h2>
        <p>Kreiraj novu sednicu sa pitanjima za trenutno aktivno zasedanje</p>
    </div>

    <!-- Current Session Info -->
    <div class="current-session-info">
        <h3>📋 Trenutno Aktivno Zasedanje</h3>
        <div class="session-details">
            <div class="detail-item">
                <div class="detail-label">Naziv Zasedanja</div>
                <div class="detail-value">
                    <asp:Label ID="lblTrenutnoZasedanje" runat="server" Text="Nema aktivnog zasedanja" />
                </div>
            </div>
            <div class="detail-item">
                <div class="detail-label">Tip Zasedanja</div>
                <div class="detail-value">
                    <asp:Label ID="lblTipZasedanja" runat="server" Text="-" />
                </div>
            </div>
            <div class="detail-item">
                <div class="detail-label">Saziv</div>
                <div class="detail-value">
                    <asp:Label ID="lblSaziv" runat="server" Text="-" />
                </div>
            </div>
            <div class="detail-item">
                <div class="detail-label">Status</div>
                <div class="detail-value">
                    <asp:Label ID="lblStatus" runat="server" Text="-" />
                </div>
            </div>
        </div>
    </div>

    <!-- Form Container -->
    <div class="form-container">
        <h3 style="text-align: center; margin-bottom: 30px; color: #495057;">📋 Kreiraj Novu Sednicu</h3>
        
        <div class="form-row">
            <div class="form-group">
                <label for="txtNazivSednice">Naziv Sednice:</label>
                <asp:TextBox ID="txtNazivSednice" runat="server" CssClass="form-control" 
                           placeholder="Unesite naziv sednice..." />
                <asp:RequiredFieldValidator ID="rfvNazivSednice" runat="server" 
                                          ControlToValidate="txtNazivSednice"
                                          ErrorMessage="Naziv sednice je obavezan!" 
                                          Display="Dynamic" ForeColor="#dc3545" />
            </div>
            
            <div class="form-group">
                <label for="txtDatumSednice">Datum Sednice:</label>
                <asp:TextBox ID="txtDatumSednice" runat="server" CssClass="form-control" 
                           TextMode="Date" />
                <asp:RequiredFieldValidator ID="rfvDatumSednice" runat="server" 
                                          ControlToValidate="txtDatumSednice"
                                          ErrorMessage="Datum sednice je obavezan!" 
                                          Display="Dynamic" ForeColor="#dc3545" />
            </div>
        </div>
        
        <div class="form-group">
            <label for="txtOpisSednice">Opis Sednice:</label>
            <asp:TextBox ID="txtOpisSednice" runat="server" CssClass="form-control" 
                       TextMode="MultiLine" Rows="4" 
                       placeholder="Unesite opis sednice..." />
        </div>
        
        <!-- Pravilo 7 dana - objašnjenje za hitne sednice -->
        <div class="form-group" id="sevenDayRuleSection" style="display: none;">
            <div class="alert alert-warning">
                <h5>⚠️ Pravilo 7 dana</h5>
                <p>
                    Sednica je zakazana manje od 7 dana unapred. Prema poslovnim pravilima, 
                    potrebno je navesti razlog za hitno sazivanje sednice.
                </p>
            </div>
            <label for="txtRazlogHitnosti">Razlog za hitno sazivanje sednice:</label>
            <asp:TextBox ID="txtRazlogHitnosti" runat="server" CssClass="form-control" 
                       TextMode="MultiLine" Rows="3" 
                       placeholder="Unesite razlog zašto je sednica hitna i zašto se saziva manje od 7 dana unapred..." />
            <small class="form-text text-muted">
                Ovo polje je obavezno kada se sednica zakazuje manje od 7 dana unapred.
            </small>
        </div>
        
        <div class="questions-section">
            <h4>📝 Pitanja za Sednicu</h4>
            <div id="questionsContainer">
                <div class="question-item">
                    <div class="question-number">1</div>
                    <asp:TextBox ID="txtPitanje1" runat="server" CssClass="form-control question-input" 
                               placeholder="Unesite pitanje..." />
                    <button type="button" class="btn-danger" onclick="removeQuestion(this)">×</button>
                </div>
            </div>
            <button type="button" class="btn-success" onclick="addQuestion()">+ Dodaj Pitanje</button>
            
            <!-- Hidden field to store all questions -->
            <asp:HiddenField ID="hdnQuestions" runat="server" />
        </div>

        <div class="form-actions">
            <asp:Button ID="btnKreirajSednicu" runat="server" Text="📅 Kreiraj Sednicu" 
                       CssClass="btn btn-primary" OnClick="btnKreirajSednicu_Click" 
                       OnClientClick="return validateForm();" />
            <asp:Button ID="btnResetuj" runat="server" Text="🔄 Resetuj" 
                       CssClass="btn btn-secondary" OnClick="btnResetuj_Click" />
        </div>
    </div>

    <!-- Help Text -->
    <div class="help-text">
        <h4>💡 Kako funkcioniše?</h4>
        <p>
            Kada kreirate novu sednicu, ona se automatski povezuje sa trenutno aktivnim zasedanjem. 
            Sednica može imati više pitanja koja se dodaju dinamički. Svako pitanje će biti dostupno 
            za glasanje poslanicima. Nakon kreiranja sednice, možete dodati dodatna pitanja ili 
            modifikovati postojeća.
        </p>
        <h5>📅 Pravilo 7 dana</h5>
        <p>
            Prema poslovnim pravilima, ako se sednica zakazuje manje od 7 dana unapred, 
            potrebno je navesti razlog za hitno sazivanje. Sistem će automatski prikazati 
            polje za unos razloga kada izaberete datum koji je manje od 7 dana od danas.
        </p>
    </div>

    <!-- Alerts -->
    <asp:Panel ID="pnlAlert" runat="server" Visible="false" CssClass="alert">
        <asp:Label ID="lblAlertMessage" runat="server" />
    </asp:Panel>

    <script>
        let questionCounter = 2;

        function addQuestion() {
            const container = document.getElementById('questionsContainer');
            const newQuestion = document.createElement('div');
            newQuestion.className = 'question-item';
            newQuestion.innerHTML = `
                <div class="question-number">${questionCounter}</div>
                <input type="text" class="form-control question-input" placeholder="Unesite pitanje..." />
                <button type="button" class="btn-danger" onclick="removeQuestion(this)">×</button>
            `;
            container.appendChild(newQuestion);
            questionCounter++;
            
            // Add event listener to the new input
            const newInput = newQuestion.querySelector('.question-input');
            newInput.addEventListener('input', updateQuestionsHiddenField);
            
            updateQuestionsHiddenField();
        }

        function removeQuestion(button) {
            const questionItem = button.parentElement;
            if (document.querySelectorAll('.question-item').length > 1) {
                questionItem.remove();
                // Renumber questions
                const questions = document.querySelectorAll('.question-item');
                questions.forEach((item, index) => {
                    const numberDiv = item.querySelector('.question-number');
                    if (numberDiv) {
                        numberDiv.textContent = index + 1;
                    }
                });
                questionCounter = questions.length + 1;
                updateQuestionsHiddenField();
            }
        }

        function updateQuestionsHiddenField() {
            const questions = [];
            const questionInputs = document.querySelectorAll('.question-input');
            
            questionInputs.forEach((input, index) => {
                if (input.value.trim() !== '') {
                    questions.push(input.value.trim());
                }
            });
            
            // Update hidden field with comma-separated values
            const hiddenField = document.getElementById('<%= hdnQuestions.ClientID %>');
            if (hiddenField) {
                hiddenField.value = questions.join(',');
                console.log('Hidden field updated with:', hiddenField.value);
            }
            
            console.log('Questions updated:', questions);
        }

        function resetQuestionsContainer() {
            const container = document.getElementById('questionsContainer');
            if (container) {
                // Keep only the first question
                const questions = container.querySelectorAll('.question-item');
                for (let i = 1; i < questions.length; i++) {
                    questions[i].remove();
                }
                
                // Clear the first question
                const firstQuestion = container.querySelector('.question-input');
                if (firstQuestion) {
                    firstQuestion.value = '';
                }
                
                // Reset counter
                questionCounter = 2;
                
                // Update hidden field
                updateQuestionsHiddenField();
                
                console.log('Questions container reset');
            }
        }

        // Add event listeners to existing questions and ensure hidden field is updated before form submission
        document.addEventListener('DOMContentLoaded', function() {
            const questionInputs = document.querySelectorAll('.question-input');
            questionInputs.forEach(input => {
                input.addEventListener('input', updateQuestionsHiddenField);
            });
            
            // Ensure hidden field is updated before form submission
            const form = document.querySelector('form');
            if (form) {
                form.addEventListener('submit', function(e) {
                    updateQuestionsHiddenField();
                    console.log('Form submitted with questions:', document.getElementById('<%= hdnQuestions.ClientID %>').value);
                });
            }
            
            // Also update when the create button is clicked
            const createButton = document.getElementById('<%= btnKreirajSednicu.ClientID %>');
            if (createButton) {
                createButton.addEventListener('click', function(e) {
                    updateQuestionsHiddenField();
                    console.log('Create button clicked with questions:', document.getElementById('<%= hdnQuestions.ClientID %>').value);
                });
            }
        });

        // Set default date to today
        document.addEventListener('DOMContentLoaded', function() {
            const today = new Date().toISOString().split('T')[0];
            const dateInput = document.getElementById('<%= txtDatumSednice.ClientID %>');
            if (dateInput) {
                dateInput.value = today;
            }
            
            // Proveri pravilo 7 dana na učitavanju stranice
            checkSevenDayRule();
            
            // Dodaj event listener za datum da proveri pravilo 7 dana
            if (dateInput) {
                dateInput.addEventListener('change', checkSevenDayRule);
                dateInput.addEventListener('input', checkSevenDayRule);
            }
        });

        // Funkcija za proveru pravila 7 dana
        function checkSevenDayRule() {
            const dateInput = document.getElementById('<%= txtDatumSednice.ClientID %>');
            const sevenDaySection = document.getElementById('sevenDayRuleSection');
            const reasonInput = document.getElementById('<%= txtRazlogHitnosti.ClientID %>');
            
            if (dateInput && sevenDaySection && reasonInput) {
                const selectedDate = new Date(dateInput.value);
                const today = new Date();
                const timeDiff = selectedDate.getTime() - today.getTime();
                const daysDiff = Math.ceil(timeDiff / (1000 * 3600 * 24));
                
                console.log('Razlika u danima:', daysDiff);
                
                // Ako je manje od 7 dana, prikaži sekciju za razlog
                if (daysDiff < 7) {
                    sevenDaySection.style.display = 'block';
                    reasonInput.setAttribute('required', 'required');
                    console.log('Prikazujem sekciju za pravilo 7 dana');
                } else {
                    sevenDaySection.style.display = 'none';
                    reasonInput.removeAttribute('required');
                    reasonInput.value = '';
                    console.log('Sakrivam sekciju za pravilo 7 dana');
                }
            }
        }

        // Funkcija za validaciju forme pre slanja
        function validateForm() {
            const dateInput = document.getElementById('<%= txtDatumSednice.ClientID %>');
            const reasonInput = document.getElementById('<%= txtRazlogHitnosti.ClientID %>');
            
            if (dateInput && reasonInput) {
                const selectedDate = new Date(dateInput.value);
                const today = new Date();
                const timeDiff = selectedDate.getTime() - today.getTime();
                const daysDiff = Math.ceil(timeDiff / (1000 * 3600 * 24));
                
                // Ako je manje od 7 dana i nema razloga, prikaži grešku
                if (daysDiff < 7 && (!reasonInput.value || reasonInput.value.trim() === '')) {
                    alert('Greška: Kada se sednica zakazuje manje od 7 dana unapred, potrebno je navesti razlog za hitno sazivanje.');
                    reasonInput.focus();
                    return false;
                }
            }
            
            return true;
        }
    </script>
</asp:Content>
