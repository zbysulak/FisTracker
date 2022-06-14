<template>
    <v-expansion-panels focusable>
        <v-expansion-panel>
        <v-expansion-panel-header>Jak má vypadat screenshot docházky pro nahrátí ?</v-expansion-panel-header>
        <v-expansion-panel-content>
            Snímek obrazovky udělejte ideálně tak, aby byla na snímku vidět pouze tabulka s datumy a časy. Vyhněte se zbytečně velkým okrajům okolo tabulky a dalším textům, které mohou narušit správné rozpoznání hodnot. Nedoporučujeme dělat screenshot celé obrazovky, ale pouze daného výřezu s tabulkou.
        </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
        <v-expansion-panel-header>Jak se správně zaregistrovat ?</v-expansion-panel-header>
        <v-expansion-panel-content>
            Klikněte na tlačítko "Register" na hlavní stránce. Vyplňte Vaše jméno či přezdívku a heslo. Klikněte na "Register". Váš účet je nyní vytvořen a budete přesěrování na úvodní stránku aplikace.
        </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
        <v-expansion-panel-header>Jak se odhlásím ?</v-expansion-panel-header>
        <v-expansion-panel-content>
            Aplikace si pamatuje přihlášení 1 týden po posledním přihlášení. Poté se budete muset přihlásit znovu. Pokud si přejete se odhlásit dříve, stačí kliknout na ikonku "dveří" v pravém horním rohu.
        </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
        <v-expansion-panel-header>Jak si zobrazím přestávky na oběd ?</v-expansion-panel-header>
        <v-expansion-panel-content>
            V horní liště klikněte na tlačítko "Settings" a zaškrtněte "Lunch break". Pokud naopak chcete přestávky na oběd skrýt, tak odškrtněte tuto položku. Vždy musíte volbu potvrdit tlačítkem "Save" a být přihlášeni do svého účtu.
        </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
        <v-expansion-panel-header>Jak skryji nepracovní dny ?</v-expansion-panel-header>
        <v-expansion-panel-content>
            V horní liště klikněte na tlačítko "Settings" a odškrtněte "Show non-workings days". Pokud naopak chcete nepracovní dny zobrazit, tak zaškrtněte tuto položku. Vždy musíte volbu potvrdit tlačítkem "Save" a být přihlášeni do svého účtu.
        </v-expansion-panel-content>
        </v-expansion-panel>
        <v-expansion-panel>
        <v-expansion-panel-header>Jak funguje toggl "Overwrite existing" ?</v-expansion-panel-header>
        <v-expansion-panel-content>
            Pokud nahráváte docházku pomocí snímku obrazovky, tak máte možnost nahrát snímek i s docházkou, kterou již máte zadanou, v takovém případě se docházka přepíše novejší, pokud máte volbu "Overwrite existing" povolenou. V opačném případě se přidá docházka pouze k novým dnům.
        </v-expansion-panel-content>
        </v-expansion-panel>
    </v-expansion-panels>
</template>

<script>
export default {
  name: "faq",
  data: () => ({
    
  })
}
</script>