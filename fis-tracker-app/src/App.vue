<template>
  <v-app>
    <v-app-bar app color="primary" dark>
      <div class="d-flex align-center">
        <v-img
          alt="Vuetify Logo"
          class="shrink mr-2"
          contain
          src="https://cdn.vuetifyjs.com/images/logos/vuetify-logo-dark.png"
          transition="scale-transition"
          width="40" />

        <span class="font-weight-bold">FisTracker</span>
      </div>
      <v-spacer></v-spacer>
      <v-row class="justify-end">
        <settings />
        <Login />
      </v-row>
    </v-app-bar>
    <v-main>
      <router-view></router-view>
    </v-main>
    <snack ref="snack" />
  </v-app>
</template>

<script>
import Login from "./components/Login"
import Settings from "./components/Settings.vue"
import Snack from "./components/Snack.vue"
import axios from "axios"

axios.defaults.withCredentials = true
export default {
  name: "App",
  components: { Login, Settings, Snack },

  data: () => ({
    //
  }),
  mounted() {
    this.$root.snack = this.$refs.snack
  }
}
</script>
