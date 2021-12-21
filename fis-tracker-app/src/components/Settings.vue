<template>
  <v-dialog v-model="dialog" width="500">
    <template v-slot:activator="{ on, attrs }">
      <v-btn class="mr-10" text v-bind="attrs" v-on="on">
        Settings
        
      </v-btn>
    </template>
    <v-card>
      <v-card-title class="text-h5 grey lighten-2"> Settings </v-card-title>
      <v-card-text>
        <v-checkbox
          v-model="settings.defaultLunch"
          label="Lunch break"></v-checkbox>
        <v-checkbox
          v-model="settings.showNoWorkday"
          label="Show non-working days"></v-checkbox>
      </v-card-text>
      <v-divider></v-divider>
      <v-card-actions>
        <v-spacer></v-spacer
        ><v-btn color="blue darken-1" text @click="dialog = false">
          Close
        </v-btn>
        <v-btn color="primary" text @click="save"> Save </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
<script>
export default {
  data() {
    return {
      dialog: false,
      settings: {
        defaultLunch: false,
        showNoWorkday: true
      }
    }
  },
  methods: {
    save: function () {
      if (window.localStorage) {
        window.localStorage.settings = JSON.stringify(this.settings)
        this.$root.snack.show({ message: "Settings saved" })
        this.dialog = false
      }
    }
  },
  mounted() {
    if (window.localStorage.settings) {
      this.settings = JSON.parse(window.localStorage.settings)
    }
  }
}
</script>
