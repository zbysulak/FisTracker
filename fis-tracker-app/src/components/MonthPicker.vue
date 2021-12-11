<template>
  <v-menu
    ref="menu"
    v-model="menu"
    :close-on-content-click="false"
    :return-value.sync="date"
    transition="scale-transition"
    offset-y
    max-width="290px"
    min-width="auto">
    <template v-slot:activator="{ on, attrs }">
      <v-text-field
        v-model="formatted"
        :label="label"
        prepend-icon="mdi-calendar"
        readonly
        v-bind="attrs"
        v-on="on"></v-text-field>
    </template>
    <v-date-picker
      v-model="date"
      type="month"
      no-title
      scrollable
      @input="save">
    </v-date-picker>
  </v-menu>
</template>
<script>
export default {
  props: {
    label: { type: String, default: "Select month" },
    value: { type: String }
  },
  data: () => ({
    menu: false,
    date: ""
  }),
  computed: {
    formatted() {
      let d = new Date(Date.parse(this.date))
      let o = d.toLocaleString("en", { month: "long", year: "numeric" })
      o = o.charAt(0).toUpperCase() + o.slice(1)
      return o
    }
  },
  watch: {
    value(v) {
      this.date = v
    }
  },
  methods: {
    save: function () {
      this.menu = false
      this.$refs.menu.save(this.date)
      this.$emit("input", this.date)
    }
  },
  created() {
    this.date = this.value
  }
}
</script>
