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
          width="40"
        />

        <span class="font-weight-bold">FisTracker</span>
      </div>
      <v-spacer></v-spacer>
      <Login />
    </v-app-bar>

    <v-main>
      <router-view></router-view>
    </v-main>
  </v-app>
</template>

<script>
import Login from './components/Login'
export default {
  name: "App",
  components: {Login},

  data: () => ({
    //
  })
}
</script>
