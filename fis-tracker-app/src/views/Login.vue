<template>
  <v-container>
    <v-row class="text-center">
      <v-col class="mb-4">
        <h1 class="display-2 font-weight-bold mb-3">Login</h1>
      </v-col>
    </v-row>
    <v-row class="text-center">
      <form>
        <v-row>
          <v-text-field
            v-model="name"
            :error-messages="nameError"
            label="Name"
            required
            @input="$v.name.$touch()"
            @blur="$v.name.$touch()"
            class="mr-4"
          ></v-text-field>
        </v-row>
        <v-row>
          <v-text-field
            v-model="password"
            :error-messages="passError"
            label="Password"
            required
            @input="$v.password.$touch()"
            @blur="$v.password.$touch()"
            class="mr-4"
          ></v-text-field>        
        </v-row>
        <v-row>
          <v-btn class="mr-4" @click="login"> Login </v-btn>
          </v-row>
      </form>
    </v-row>
  </v-container>
</template>

<script>
import { validationMixin } from "vuelidate";
import { required } from "vuelidate/lib/validators";
import axios from 'axios';

export default {
  name: "Login",

  components: {},

  mixins: [validationMixin],

  validations: {
    name: { required },
    password: { required },
  },

  data: () => ({
    name: "",
    password: "",
    error: null,
    success: false
  }),
  computed: {

    nameError() {
      const errors = [];
      if (!this.$v.name.$dirty) return errors;
      !this.$v.name.required && errors.push("Name is required.");

      return errors;
    },

    passError() {
      const errors = [];
      if (!this.$v.password.$dirty) return errors;
      !this.$v.password.required && errors.push("Password is required.");

      return errors;
    },
  },

  methods: {
    submit() {
        this.$v.$touch();
    },

    login: async function() {
        console.log(this.name + " " + this.password)
        const auth = { name: this.name, password: this.password };
        const url = 'https://192.168.1.242:5100/api/Users/login';
        this.success = false;
        this.error = null;

        try {
            const res = await axios.post(url, auth
        ).then(res => res.data);
            this.success = true;
            console.log(res)
        } catch (err) {
            this.error = err.message;
            console.log(this.error)
        }
      }
    
  },
};
</script>
