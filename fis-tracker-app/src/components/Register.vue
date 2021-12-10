<template>
  <div>
    <v-dialog v-model="dialog" persistent max-width="600px">
      <template v-slot:activator="{ on, attrs }">
        <v-btn class="mr-10" color="blue" outlined v-bind="attrs" v-on="on">
          Register
        </v-btn>
      </template>
      <v-card>
        <v-form @submit.prevent="register">
          <v-card-title>
            <span class="text-h5">Register new account</span>
          </v-card-title>
          <v-card-text>
            <v-container>
              <v-row>
                <v-col cols="12">
                  <v-text-field
                    v-model="name"
                    :error-messages="nameError"
                    label="Name"
                    required
                    @input="$v.name.$touch()"
                    @blur="$v.name.$touch()"
                    class="mr-4"></v-text-field>
                </v-col>
                <v-col cols="12">
                  <v-text-field
                    v-model="password"
                    :error-messages="passError"
                    label="Password"
                    type="password"
                    required
                    @input="$v.password.$touch()"
                    @blur="$v.password.$touch()"
                    class="mr-4"></v-text-field>
                </v-col>
              </v-row>
            </v-container>
            <small>*indicates required field</small>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="black darken-1" text @click="dialog = false">
              Close
            </v-btn>
            <v-btn color="primary" type="submit">Register</v-btn>
          </v-card-actions>
        </v-form>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { validationMixin } from "vuelidate"
import { required } from "vuelidate/lib/validators"
import axios from "axios"

export default {
  name: "Register",

  components: {},

  mixins: [validationMixin],

  validations: {
    name: { required },
    password: { required }
  },

  data: () => ({
    name: "",
    password: "",
    dialog: false,
    error: null,
    success: false,
  }),

  computed: {
    nameError() {
      const errors = []
      if (!this.$v.name.$dirty) return errors
      !this.$v.name.required && errors.push("Name is required.")

      return errors
    },

    passError() {
      const errors = []
      if (!this.$v.password.$dirty) return errors
      !this.$v.password.required && errors.push("Password is required.")

      return errors
    }
  },

  methods: {

    submit() {
      this.$v.$touch()
    },

    register() {
      const auth = { name: this.name, password: this.password }
      const url = this.appConfig.apiUrl + "/Users/Register"
      this.success = false
      this.error = null
      axios
        .post(url, auth)
        .then((res) => {
          this.success = true
          this.dialog = false
          this.$store.state.user = res.data
          window.localStorage.token = res.data.token
          this.$root.snack.show({ message: "Successfully registred!" })
        })
        .catch((err) => {
          this.error = err.message
        })
    }
  }
}
</script>
