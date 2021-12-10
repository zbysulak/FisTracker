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
        <settings  v-if="isLogged"/>
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

export default {
  name: "App",
  components: { Login, Settings, Snack },

  data: () => ({
    isLogged: false
  }),

  beforeCreate() {
    const url = this.appConfig.apiUrl + "/Users/AuthorizationCheck"
    axios.defaults.headers = { Authorization: window.localStorage.token }
    axios
      .get(url)
      .then((res) => {
        if(res.data.token !== undefined) {
          const t = window.localStorage.token
          this.$store.state.user = { token: t }
          this.isLogged = true
        } else {
          this.$store.state.user = { token: undefined }
          this.isLogged = false
          console.log("UNDEFINED TOKEN")
        }
      })
        .catch((err) => {
          console.log("CHYBA", err)
          this.$store.state.user = { token: undefined }
      })
  },

  mounted() {
    this.$root.snack = this.$refs.snack
  },
  watch: {
    "$store.state.user": {
      handler: function () {
        axios.defaults.headers = { Authorization: this.$store.state.user.token }
      },
      immediate: true // provides initial (not changed yet) state
    }
  }
}
</script>
