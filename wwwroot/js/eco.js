document.addEventListener('DOMContentLoaded', function () {

    /* ================================================
       ЭКРАН 1: ҚАЛА ТАҢДАУ
    ================================================ */
    const citySelect   = document.querySelector('select[name="City"]');
    const cityInfo     = document.getElementById('city-info');
    const citiesDataEl = document.getElementById('cities-data');

    if (citySelect && citiesDataEl) {
        const cities = JSON.parse(citiesDataEl.textContent);
        citySelect.addEventListener('change', function () {
            const selected = cities.find(c => c.name === this.value);
            if (!selected) { cityInfo && cityInfo.classList.add('hidden'); return; }
            document.getElementById('ci-energy').textContent    = `${selected.coal}% — көмірден алынады`;
            document.getElementById('ci-waste').textContent     = `${selected.waste}% — өңделеді`;
            document.getElementById('ci-transport').textContent = `${selected.transport}/100 ұпай`;
            cityInfo && cityInfo.classList.remove('hidden');
        });
        if (citySelect.value) citySelect.dispatchEvent(new Event('change'));
    }

    /* ================================================
       RADIO / CHECKBOX КАРТОЧКАЛАРЫ
    ================================================ */
    document.querySelectorAll('.radio-card input[type="radio"]').forEach(function (radio) {
        radio.addEventListener('change', function () {
            document.querySelectorAll(`input[name="${this.name}"]`).forEach(r => {
                r.closest('.radio-card').classList.remove('selected');
            });
            this.closest('.radio-card').classList.add('selected');
        });
        if (radio.checked) radio.closest('.radio-card').classList.add('selected');
    });

    document.querySelectorAll('.checkbox-card input[type="checkbox"]').forEach(function (cb) {
        cb.addEventListener('change', function () {
            this.closest('.checkbox-card').classList.toggle('selected', this.checked);
        });
        if (cb.checked) cb.closest('.checkbox-card').classList.add('selected');
    });

    /* ================================================
       ЭКРАН 2: СЛАЙДЕРЛЕР
    ================================================ */

    // Ұшу
    const flightsRange = document.getElementById('flights-range');
    const flightsVal   = document.getElementById('flights-val');
    const flightsDesc  = document.getElementById('flights-desc');
    if (flightsRange) {
        function updateFlights(v) {
            flightsVal.textContent = v;
            if (flightsDesc) flightsDesc.textContent =
                v == 0  ? 'Ұшақпен ұшпаймыз' :
                v <= 2  ? 'Сирек саяхат — экоға жақын' :
                v <= 6  ? 'Орташа саяхат деңгейі' :
                v <= 12 ? 'Жиі ұшу — CO₂ деңгейі жоғары' :
                          '✈️ Өте жиі ұшу — климатқа айтарлықтай әсер';
        }
        flightsRange.addEventListener('input', function () { updateFlights(this.value); });
        updateFlights(flightsRange.value);
    }

    // Кондиционер
    const acRange = document.getElementById('ac-range');
    const acVal   = document.getElementById('ac-val');
    if (acRange) {
        acRange.addEventListener('input', function () { acVal.textContent = this.value + ' ай'; });
    }

    // ── МИКРО СЛАЙДЕРЛЕР ──────────────────────────────────
    const coffeeRange  = document.getElementById('coffee-range');
    const coffeeVal    = document.getElementById('coffee-val');
    const coffeeCalc   = document.getElementById('coffee-calc');
    const coffeeText   = document.getElementById('coffee-calc-text');

    const shoppingRange = document.getElementById('shopping-range');
    const shoppingVal   = document.getElementById('shopping-val');
    const shoppingCalc  = document.getElementById('shopping-calc');
    const shoppingText  = document.getElementById('shopping-calc-text');

    const previewCoffee = document.getElementById('preview-coffee');
    const previewBags   = document.getElementById('preview-bags');
    const previewTotal  = document.getElementById('preview-total');

    function updateMicroPreview() {
        if (!coffeeRange || !shoppingRange) return;

        const cups   = parseInt(coffeeRange.value)  || 0;
        const trips  = parseInt(shoppingRange.value) || 0;
        const coffeeGr  = cups  * 12 * 15;
        const bagsGr    = trips * 52 * 8;
        const totalKg   = ((coffeeGr + bagsGr) / 1000).toFixed(2);

        if (coffeeVal)  coffeeVal.textContent  = cups;
        if (shoppingVal) shoppingVal.textContent = trips;

        // Кофе есебі
        const coffeeGrText = coffeeGr >= 1000
            ? `${(coffeeGr/1000).toFixed(2)} кг` : `${coffeeGr} гр`;
        if (coffeeText) coffeeText.textContent = `Жылына: ${coffeeGrText} пластик`;
        if (coffeeCalc) coffeeCalc.classList.toggle('has-value', cups > 0);

        // Пакет есебі
        const bagsGrText = bagsGr >= 1000
            ? `${(bagsGr/1000).toFixed(2)} кг` : `${bagsGr} гр`;
        if (shoppingText) shoppingText.textContent = `Жылына: ${bagsGrText} полиэтилен`;
        if (shoppingCalc) shoppingCalc.classList.toggle('has-value', trips > 0);

        // Preview панель
        if (previewCoffee) previewCoffee.textContent = coffeeGrText + '/жыл';
        if (previewBags)   previewBags.textContent   = bagsGrText + '/жыл';
        if (previewTotal)  previewTotal.textContent  = totalKg + ' кг/жыл';
    }

    if (coffeeRange)   coffeeRange.addEventListener('input',   updateMicroPreview);
    if (shoppingRange) shoppingRange.addEventListener('input', updateMicroPreview);
    updateMicroPreview();

    /* ================================================
       ЭКРАН 3: ЖҮЙЕЛІК СИМУЛЯТОР
    ================================================ */
    if (typeof state !== 'undefined') {
        window.toggleSwitch = function (type) {
            state[type] = !state[type];
            const toggle = document.getElementById('toggle-' + type);
            const impact = document.getElementById('impact-' + type);
            const card   = document.getElementById('card-' + type);
            toggle && toggle.classList.toggle('on', state[type]);
            impact && impact.classList.toggle('hidden', !state[type]);
            card   && card.classList.toggle('active', state[type]);
            recalcSystem();
        };

        function recalcSystem() {
            fetch(`/Survey/GetSimulationScore?id=${resultId}` +
                  `&greenEnergy=${state.green}&publicTransport=${state.transport}&localProduction=${state.local}`)
            .then(r => r.json())
            .then(data => {
                if (!data.success) return;
                const simScore     = document.getElementById('sim-score');
                const simReduction = document.getElementById('sim-reduction');
                const simRedVal    = document.getElementById('sim-red-val');

                simScore.style.transform = 'scale(1.1)';
                simScore.textContent = data.score;
                setTimeout(() => simScore.style.transform = 'scale(1)', 200);

                if (data.reduction > 0) {
                    simReduction.classList.remove('hidden');
                    simRedVal.textContent = `${data.reduction} балл`;
                } else {
                    simReduction.classList.add('hidden');
                }

                simScore.style.color =
                    data.score <= 30 ? 'var(--accent)'  :
                    data.score <= 55 ? 'var(--warn)'    :
                    data.score <= 75 ? '#e08820'        : 'var(--danger)';

                const anyOn = state.green || state.transport || state.local;
                const combinedRow = document.getElementById('combined-row');
                const combinedBar = document.getElementById('combined-bar');
                const combinedVal = document.getElementById('combined-val');
                if (combinedRow) combinedRow.style.display = anyOn ? 'flex' : 'none';
                if (combinedBar) combinedBar.style.width = data.score + '%';
                if (combinedVal) combinedVal.textContent  = data.score;
            })
            .catch(console.error);
        }
    }

    /* ================================================
       ЭКРАН 3: МИКРО-СИМУЛЯТОР
    ================================================ */
    if (typeof microState !== 'undefined') {
        window.toggleMicro = function (type) {
            microState[type] = !microState[type];
            const toggle = document.getElementById('toggle-' + type);
            const card   = document.getElementById('mcard-' + type);
            toggle && toggle.classList.toggle('on', microState[type]);
            card   && card.classList.toggle('active', microState[type]);
            recalcMicro();
        };

        function recalcMicro() {
            fetch(`/Survey/GetMicroScore?id=${resultId}` +
                  `&tumbler=${microState.tumbler}&shopper=${microState.shopper}`)
            .then(r => r.json())
            .then(data => {
                if (!data.success) return;

                const savedEl     = document.getElementById('micro-saved');
                const remainEl    = document.getElementById('micro-remaining');
                const progressEl  = document.getElementById('micro-progress');
                const pctEl       = document.getElementById('micro-pct');
                const messageEl   = document.getElementById('micro-message');
                const messageText = document.getElementById('micro-message-text');

                if (savedEl)  savedEl.textContent   = data.plasticSaved.toFixed(2) + ' кг';
                if (remainEl) remainEl.textContent   = data.remaining.toFixed(2) + ' кг';

                const pct = totalPlasticKg > 0
                    ? Math.round(data.plasticSaved / totalPlasticKg * 100) : 0;
                if (progressEl) progressEl.style.width = pct + '%';
                if (pctEl)      pctEl.textContent      = pct + '% үнемделді';

                if (messageEl && messageText) {
                    if (data.message) {
                        messageEl.classList.remove('hidden');
                        messageText.innerHTML = data.message;
                    } else {
                        messageEl.classList.add('hidden');
                    }
                }
            })
            .catch(console.error);
        }
    }

    /* ================================================
       БАР-ЧАРТ АНИМАЦИЯСЫ
    ================================================ */
    document.querySelectorAll('.sb-fill, .cc-bar').forEach(function (el) {
        const w = el.style.width;
        el.style.width = '0%';
        requestAnimationFrame(() => setTimeout(() => el.style.width = w, 100));
    });
});
