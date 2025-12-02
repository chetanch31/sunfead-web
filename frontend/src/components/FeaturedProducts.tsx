import ProductCard from "./ProductCard";
import { Link } from "react-router-dom";

const products = [
	{
		id: "3",
		name: "Fusilli Pasta",
		image: "/static/products/Fusilli_Pasta_10_80gms.jpg",
		category: "Pasta",
		variants: [
			{ id: "3-1", weight: "80gm", mrp: 10, price: 10 },
			{ id: "3-2", weight: "80gm", mrp: 450, price: 450 },
			{ id: "3-3", weight: "120gm", mrp: 500, price: 500 },
		],
	},
	{
		id: "4",
		name: "Macaroni",
		image: "/static/products/Macroni_10_80gms_page-0001.jpg",
		category: "Pasta",
		variants: [
			{ id: "4-1", weight: "80gm", mrp: 10, price: 10 },
			{ id: "4-2", weight: "120gm", mrp: 500, price: 500 },
		],
	},
	{
		id: "9",
		name: "Roasted Vermicelli",
		image: "/static/products/roastedvermicelli_10_75gm.jpg",
		category: "Vermicelli",
		variants: [
			{ id: "9-1", weight: "75gm", mrp: 10, price: 10 },
			{ id: "9-2", weight: "450gm", mrp: 70, price: 70 },
		],
	},
	{
		id: "10",
		name: "Poha",
		image: "/static/products/poha_10_100gm.jpg",
		category: "Flour & Grains",
		variants: [
			{ id: "10-1", weight: "100gm", mrp: 10, price: 10 },
			{ id: "10-2", weight: "500gm", mrp: 70, price: 70 },
		],
	},
];

const FeaturedProducts = () => {
	return (
		<section className="py-16 md:py-24 bg-background">
			<div className="container mx-auto px-4">
				<div className="text-center mb-12">
					<h2 className="text-3xl md:text-4xl lg:text-5xl font-display font-bold mb-4">
						Bestsellers
					</h2>
					<p className="text-lg text-muted-foreground max-w-2xl mx-auto">
						Our most loved treats, crafted with authentic recipes passed down
						through generations
					</p>
				</div>

				<div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
					{products.map((product) => (
						<ProductCard key={product.id} {...product} />
					))}
				</div>

				<div className="text-center mt-12">
					<Link
						to="/shop"
						className="inline-block text-primary font-semibold hover:underline"
					>
						View All Products →
					</Link>
				</div>
			</div>
		</section>
	);
};

export default FeaturedProducts;
